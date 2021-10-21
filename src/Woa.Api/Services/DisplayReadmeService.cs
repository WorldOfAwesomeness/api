using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Woa.Api.Services;

public sealed class DisplayReadmeService
    : ServiceContractBase<DisplayReadmeRequest, MarkdownResult>
{
    public override string RelativePath => "/DisplayReadme";
    public override Methods Method { get; } = Methods.Get;

    public DisplayReadmeService(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    internal override async Task<MarkdownResult?> Operation(
        HttpContext context,
        DisplayReadmeRequest? request)
    {
        var found = Items.TryGetValue(request?.Id ?? -1, out var item);
        
        if(request is null || !found)
        {
            context.Response.Redirect("/");
            return null;
        }

        request!.Item = item;

        StringBuilder markdown = new();

        markdown.AppendLine("# Search Results of Awesomeness");

        var gitHub = ServiceProvider.GetService<GitHubReadmeService>()!;

        var result = await gitHub.Operation(context, request);

        if (result is not null)
        {
            BuildResults(markdown, result);

            var bytes = Convert.FromBase64String(result.ReadmeData?.Content ?? "");
            var md = Encoding.UTF8.GetString(bytes);
            var html = MarkdownResponse.Create(md).ToHtml();

            markdown.AppendLine("<article class='markdown-body entry-content container-lg'>");
            markdown.AppendLine(html);
            markdown.AppendLine("</article>");

            //markdown.AppendLine("<details><summary>json</summary>");
            //markdown.AppendLine("\n```md");
            //markdown.AppendLine(md);
            //markdown.AppendLine("```\n");
            //markdown.AppendLine("</details>");
        }
        else
        {
            markdown.AppendLine("<div class='alert alert-danger'>Search response is null.</div>");
        }

        return new MarkdownResult(markdown.ToString());
    }

    private void BuildResults(StringBuilder sb, GitHubReadmeResult result)
    {
        if (result.Data is ReadmeResult readmeResult)
        {
            sb.AppendLine($"[{readmeResult.Name} Readme]({readmeResult.HtmlUrl})");
        }
    }

    private static ConcurrentDictionary<int, Item> Items => SearchResultsPageService.Items;
}