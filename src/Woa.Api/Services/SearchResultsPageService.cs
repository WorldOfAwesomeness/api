namespace Woa.Api.Services;

public sealed class SearchResultsPageService
    : ServiceContractBase<GitHubSearchRequest, MarkdownResult>
{
    public override string RelativePath => "/SearchResults";

    public override Methods Method { get; } = Methods.Post;

    public SearchResultsPageService(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    internal override async Task<MarkdownResult?> Operation(
        HttpContext context,
        GitHubSearchRequest? request)
    {
        StringBuilder markdown = new();

        markdown.AppendLine("# Search Results of Awesomeness");

        var gitHub = ServiceProvider.GetService<GitHubSearchService>()!;

        var result = await gitHub.Operation(context, request);

        if (result is not null)
        {
            BuildResults(markdown, result);

            markdown.AppendLine("<details><summary>json</summary>");
            markdown.AppendLine("\n```json");
            markdown.AppendLine(result.SerializedData);
            markdown.AppendLine("```\n");
            markdown.AppendLine("</details>");
        }
        else
        {
            markdown.AppendLine("<div class='alert alert-danger'>Search response is null.</div>");
        }

        return new MarkdownResult(markdown.ToString(),request.SidebarContents);
    }

    private void BuildResults(StringBuilder sb, GitHubSearchResult result)
    {
        if (result.Data is RepositorySearchResult repoResult and { Items: not null })
        {
            repoResult
                .Items
                .Select(item => (s: $"* [{item.Name}](/DisplayReadme?id={item.Id}): {item.Description}", i: item))
                .ToList()
                .ForEach(tuple =>
                {
                    var (s, i) = tuple;

                    Items.AddOrUpdate((int)(i.Id!), i!, (_, _) => i);

                    sb.AppendLine(s);
                });


        }
    }
}