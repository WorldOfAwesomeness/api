using Markdig.Extensions.Hardlines;
using Microsoft.AspNetCore.Http.Features;

namespace Woa.Api.Common;

[ServiceContract]
public abstract class ServiceContractBase<TRequest, TResult>
    : IServiceContract
        where TRequest : DataContractBase
        where TResult : IResult, new()
{
    public static IServiceProvider ServiceProvider
    {
        get; set;
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
                        IndexPageService idx => (TRequest?)(object)new DisplayReadmeRequest(),
                        _ => default
                    };
                    break;
            }

            var result = await Operation(context, request);

            if (result is not null)
            {
                if (result is MarkdownResult mr)
                {
                    mr.SidebarMarkdown = BuildSidebar();
                }

                await result!.ExecuteAsync(context).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            await SendErrorResponse(ex, context);
        }
    }

    protected virtual string? BuildSidebar()
    {
        var sb = new StringBuilder();

        foreach (var item in Items.OrderBy(p => p.Value.Name).ThenBy(p => p.Value.Owner))
        {
            sb.AppendLine($"* [{item.Value.Owner?.Login}/{item.Value.Name}](/DisplayReadme?id={item.Value.Id})");
        }

        return sb.ToString();
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
    internal static ConcurrentDictionary<int, Item>? Items 
        => ServiceProvider.GetService<ConcurrentDictionary<int, Item>>();
}
