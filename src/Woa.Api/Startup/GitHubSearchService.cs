using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Woa.Api.Common;

namespace Woa.Api.Startup
{
    public class GitHubSearchService : ServiceContractBase<GitHubSearchRequest, GitHubSearchResult>
    {
        public GitHubSearchService(
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        protected override async Task<DataContractResponseBase?> Operation(HttpContext context, GitHubSearchRequest? request)
        {
            var httpClient = ServiceProvider.GetService<HttpClient>();

            httpClient!.AddHeaders();

            var uri = GitHubSearchUri with
            {
                QueryString = QueryString.Create("q", request?.Query ?? "topic:Awesome"),
            };

            var result = await httpClient!.GetStringAsync(uri).ConfigureAwait(false);

            var data = result.FromJson<RepositorySearchResult>();

            data!.items =
                data.items
                    .GroupBy(i => i.name.StartsWith("Awesome", StringComparison.InvariantCultureIgnoreCase))
                    .OrderByDescending(g => g.Key)
                    .SelectMany(g => g.OrderBy(i => i.name).ThenBy(i => i.full_name))
                    .Take(100)
                    .ToArray();

            data.item_count = data.items.Length;

            return new GitHubSearchResult(ResultStatuses.Success)
            {
                Data = data,
            };
        }

        public UriBase GitHubSearchUri
        {
            get;
        } =
            new("https://api.github.com/search/repositories");
    }
}