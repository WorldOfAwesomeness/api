// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Woa.MarkdownServer.Common;

#pragma warning disable PH_S025 // Unused Synchronous Task Result
// ReSharper disable once ClassNeverInstantiated.Global
public record MarkdownResponse
{
    private string? _layout = null;
    private string? _markdown = null;

    public MarkdownServerOptions? Options => MarkdownServerOptions.Current;
    public Exception? Error { get; }
    public HttpStatusCode StatusCode { get; }

    public MarkdownResponse()
    {
        StatusCode = HttpStatusCode.OK;
    }

    private MarkdownResponse(string markdown, MarkdownServerOptions? options = null) : this()
    {
        SetMarkdown(markdown);
    }

    public MarkdownResponse(HttpStatusCode statusCode, MarkdownServerOptions? options = null) : this()
    {
        StatusCode = statusCode;
    }

    public MarkdownResponse(Exception error, MarkdownServerOptions? options = null) : this()
    {
        Error = error;
        StatusCode = HttpStatusCode.InternalServerError;
    }

    private void SetMarkdown(string markdown)
    {
        _markdown = markdown;
    }

    public static MarkdownResponse Create(string markdown)
        => new(markdown);

    public static MarkdownResponse CreateFromFile(string filename)
        => new(File.ReadAllText(filename));

    public string ToHtml()
    {
        var pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseSyntaxHighlighting()
            .Build();

        var html = Markdown.ToHtml(_markdown ?? "", pipeline);

        return html;
    }

    public string ToHtmlPage()
    {
        var html = ToHtml();

        var layout = _layout ??= File.ReadAllText(Options?.Value.LayoutFile ?? "./wwwroot/Layout.html");

        var page = layout.Replace("!MarkdownBody", html);

        return page;
    }

    public MarkdownResult ToMarkdownResult()
    {
        return new MarkdownResult(_markdown ?? "");
    }
}

#pragma warning restore PH_S025 // Unused Synchronous Task Result
