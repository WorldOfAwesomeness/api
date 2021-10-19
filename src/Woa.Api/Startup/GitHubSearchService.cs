using System.Threading.Tasks;

namespace Woa.Api.Startup
{
    public class GitHubSearchService : ServiceContractBase<GitHubSearchRequest, GitHubSearchResult>
    {
        protected override Task<GitHubSearchResult?> Operation(GitHubSearchRequest? request) 
            => Task.FromResult<GitHubSearchResult?>(new (ResultStatuses.Success));
    }
}