using System.Threading.Tasks;

namespace Woa.Api.Startup
{
    public class GitHubSearchService : ServiceContractBase<GitHubSearchRequest, GitHubSearchResult>
    {
        public GitHubSearchService()
        {
        }

        protected override Task<GitHubSearchResult?> Operation(GitHubSearchRequest request) 
            => Task.FromResult(new GitHubSearchResult(ResultStatuses.Success));
    }
}