using System.Buffers;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Woa.Api.Common;

namespace Woa.Api.Startup
{
[ServiceContract]
    public abstract class ServiceContractBase<TRequest, TResult>
        : IServiceContract
            where TRequest : DataContractBase, new()
            where TResult : DataContractResponseBase, new()
    {
        public IServiceProvider ServiceProvider
        {
            get; init;
        }

        protected ServiceContractBase(
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }


        [OperationContract]
        protected abstract Task<DataContractResponseBase?> Operation(HttpContext context, TRequest? request);

        /// <inheritdoc />
        public string RelativePath { get; } = "/Search";

        public IEndpointConventionBuilder Register(WebApplication app, Methods method, string path, int order)
        {
            var builder = app.Map(path, Proxy);
            builder.WithDisplayName($"{method}: {path}");
            builder.WithGroupName(GetType().Namespace!);
            builder.WithName(GetType().Name!);

            return builder;
        }

        private async Task Proxy(HttpContext context)
        {
            try
            {
                var body = await context.Request.BodyReader.ReadAsync();
                var json =
                    body.Buffer.IsEmpty
                        ? null
                        : Encoding.UTF8.GetString(body.Buffer.ToArray());

                var request = json is not (null or "")
                    ? json.FromJson<TRequest>()
                    : null;

                var result = await Operation(context, request);

                var ok = Results.Ok(result);

                context.Response.Headers.Add(
                    "Content-Type",
                    new StringValues(
                        new string[]
                        {
                            "application/json", "charset=utf-8",
                        }
                    )
                );

                context.Response.StatusCode = (int)result.Status;

                await context.Response.WriteAsJsonAsync(ok, ok.GetType());
            }
            catch (Exception ex)
            {
                await SendErrorResponse(ex, context);
            }
        }

        private static Task SendErrorResponse(Exception ex, HttpContext context)
        {
            var error = Results.Problem(
                new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Status = 500,
                    Title = ex.Message,
                    Type = ex.GetType()
                        .Name,
                }
            );

            context.Response.Headers.Add(
                "Content-Type",
                new StringValues(
                    new string[]
                    {
                        "application/json",
                        "charset=utf-8",
                    }
                )
            );

            context.Response.StatusCode = 500;

            return context.Response.WriteAsJsonAsync(error, error.GetType());
        }
    }
}
