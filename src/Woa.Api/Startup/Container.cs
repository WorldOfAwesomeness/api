using Microsoft.Extensions.Options;
using Woa.MarkdownServer;

namespace Woa.Api.Startup;

public static class Container
{
    public static WebApplicationBuilder AddServiceContracts(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<HttpContext>(
            provider => provider.GetService<HttpContextAccessor>()!.HttpContext!
        );
        
        builder.Services.AddTransient<SearchResultsPageService>();
        builder.Services.AddTransient<IndexPageService>();
        builder.Services.AddTransient<GitHubSearchService>();
        builder.Services.AddTransient<DisplayReadmeService>();
        builder.Services.AddTransient<GitHubReadmeService>();

        builder.Services.AddTransient<IServiceContract, SearchResultsPageService>();
        builder.Services.AddTransient<IServiceContract, IndexPageService>();
        builder.Services.AddTransient<IServiceContract, GitHubSearchService>();
        builder.Services.AddTransient<IServiceContract, DisplayReadmeService>();
        builder.Services.AddTransient<IServiceContract, GitHubReadmeService>();

        //new OptionsBuilder<MarkdownServerOptions>(builder.Services, "MarkdownServer");
        builder.Services.AddOptions<MarkdownServerOptions>("MarkdownServer");
        builder.Services.AddSingleton<ConcurrentDictionary<int, Item>>();

        return builder;
    }
}
