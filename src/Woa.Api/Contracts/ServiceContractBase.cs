using System.Buffers;
using System.ComponentModel;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Woa.Api.Common;

namespace Woa.Api.Startup
{
    public interface IServiceContract
    {
        IEndpointConventionBuilder Register(WebApplication app,
                                            Methods method,
                                            string path,
                                            int order);
    }

    [ServiceContract]
    public abstract class ServiceContractBase<TRequest, TResult>
        : IServiceContract
            where TRequest : DataContractBase, new()
            where TResult : DataContractResponseBase, new()
    {
        [OperationContract]
        protected abstract Task<TResult?> Operation(TRequest? request);

        public IEndpointConventionBuilder Register(WebApplication app, Methods method, string path, int order)
        {
            var builder = EndpointRouteBuilderExtensions.Map(app, path, Proxy);
            builder.WithDisplayName($"{method}: {path}");
            builder.WithGroupName(GetType().Namespace!);
            builder.WithName(GetType().Name!);

            return builder;
        }

        private async Task<IResult> Proxy(HttpContext context)
        {
            var body = await context.Request.BodyReader.ReadAsync();
            var json =
                body.Buffer.IsEmpty
                    ? null
                    : Encoding.UTF8.GetString(body.Buffer.ToArray());

            var request = json is not (null or "")
                ? json.FromJson<TRequest>()
                : null;

            var result = await Operation(request);

            var output = result.ToJson();

            var ok = Results.Ok(output);

            context.Response.Headers.Add("Content-Type", 
                new StringValues(new string[] 
                {
                    "application/json", 
                    "charset=utf-8"
                }
            ));

            return ok;
        }
    }
}
