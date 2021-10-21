namespace Woa.MarkdownServer.Common;

public class MarkdownResult : IResult
{
    private readonly string _markdown;

    public MarkdownResult()
    {
        _markdown = "";
    }

    public MarkdownResult(string markdown)
    {
        _markdown = markdown;
    }

    /// <summary>Write an HTTP response reflecting the result.</summary>
    /// <param name="httpContext">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
    /// <returns>A task that represents the asynchronous execute operation.</returns>
    public Task ExecuteAsync(HttpContext context)
    {
        var html = MarkdownResponse.Create(_markdown).ToHtmlPage();
        var memory = new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(html));
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = "text/html";
        context.Response.ContentLength = html.Length;
        return context.Response.BodyWriter.WriteAsync(memory).AsTask();
    }
}