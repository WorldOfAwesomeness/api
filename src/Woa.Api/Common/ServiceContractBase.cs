using Markdig.Extensions.Hardlines;

namespace Woa.Api.Common;

[ServiceContract]
public abstract class ServiceContractBase<TRequest, TResult>
    : IServiceContract
        where TResult : IResult, new()
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
    internal abstract Task<TResult?> Operation(HttpContext context, TRequest? request);

    /// <inheritdoc />
    public virtual string RelativePath { get; } = "/";

    public virtual Methods Method { get; } = Methods.Get;

    public IEndpointConventionBuilder Register(WebApplication app, int order)
    {
        var builder = app.Map(RelativePath, Proxy);
        builder.WithDisplayName($"{Method}: {RelativePath}");
        builder.WithGroupName(GetType().Namespace!);
        builder.WithName(GetType().Name);

        return builder;
    }

    private async Task Proxy(HttpContext context)
    {
        try
        {
            var body = await context.Request.BodyReader.ReadAsync();
            var bodyContents =
                body.Buffer.IsEmpty
                    ? null
                    : Encoding.UTF8.GetString(body.Buffer.ToArray());

            TRequest? request = default;
            switch (context.Request.ContentType)
            {
                case "application/x-www-form-urlencoded":

                    var requestTypeName = GetType().BaseType.GenericTypeArguments.FirstOrDefault()?.Name;

                    request = requestTypeName switch
                    {
                        nameof(DisplayReadmeRequest) => (TRequest?)(object)new DisplayReadmeRequest() { BodyContents = bodyContents },
                        nameof(GitHubSearchRequest) => (TRequest?)(object)new GitHubSearchRequest() { BodyContents = bodyContents },
                        _ => default
                    };
                    break;
                
                case "application/json":
                    request = bodyContents is not (null or "")
                        ? bodyContents.FromJson<TRequest>()
                        : default;
                    break;

                case null:
                    request = this switch
                    {
                        DisplayReadmeService drs => (TRequest?)(object)new DisplayReadmeRequest { BodyContents = context.Request.Query["id"][0] },
                        _ => default
                    };
                    break;
            }

            var result = await Operation(context, request);

            if (result is not null)
            {
                await result!.ExecuteAsync(context).ConfigureAwait(false);
            }
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
                new[]
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
