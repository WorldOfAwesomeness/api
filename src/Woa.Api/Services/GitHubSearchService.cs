using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Woa.Api.Services;

public class GitHubSearchService 
    : ServiceContractBase<GitHubSearchRequest, GitHubSearchResult>
{
    private const string HTTPS_API_GITHUB_COM_SEARCH_REPOSITORIES 
        = "https://api.github.com/search/repositories";

    public override string RelativePath => "/api/Search";

    public GitHubSearchService(
        IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    internal override async Task<GitHubSearchResult?> Operation(
        HttpContext context, 
        GitHubSearchRequest? request)
    {
        var httpClient = ServiceProvider.GetService<HttpClient>();

        httpClient!.AddHeaders();

        var q = request?.Query?.Split('=').LastOrDefault() ?? "topic:Awesome";

        q = WebUtility.UrlDecode(q);

        var uri = GitHubSearchUri with
        {
            QueryString = QueryString.Create("q", q),
        };

        var result = await httpClient!.GetStringAsync(uri).ConfigureAwait(false);

        var data = result.FromJson<RepositorySearchResult>();

        data!.Items =
            data.Items?
                .GroupBy(i => i.Name?.StartsWith("Awesome", StringComparison.InvariantCultureIgnoreCase) ?? false)
                .OrderByDescending(g => g.Key)
                .SelectMany(g => g.OrderBy(i => i.Name).ThenBy(i => i.FullName))
                .Take(100)
                .ToArray();

        data.ItemCount = data.Items?.Length ?? 0;

        return new GitHubSearchResult(ResultStatuses.Success)
        {
            Data = data,
        };
    }

    private UriBase GitHubSearchUri
    {
        get;
    } = new(HTTPS_API_GITHUB_COM_SEARCH_REPOSITORIES);
}