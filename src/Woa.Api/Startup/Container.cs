using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Woa.Api.Startup
{
    public static class Container
    {
        public static WebApplicationBuilder AddServiceContracts(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<HttpContext>(
                provider => provider.GetService<HttpContextAccessor>()!.HttpContext!
            );
            builder.Services.AddTransient<IServiceContract, GitHubSearchService>();

            return builder;
        }
    }
}
