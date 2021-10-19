using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Woa.Api.Startup
{
    public static class Container
    {
        public static WebApplicationBuilder AddServiceContracts(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IServiceContract, GitHubSearchService>();

            return builder;
        }
    }
}
