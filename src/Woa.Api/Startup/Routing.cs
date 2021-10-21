namespace Woa.Api.Startup;

public static class Routing
{
    public static WebApplication UseRoute(this WebApplication app)
    {
        //app.MapGet("/", _ => MarkdownResult.Create("# World of Awesomeness"));
        var services = app.Services.GetServices<IServiceContract>();

        var index = 1;
        foreach (var service in services)
        {
            service.Register(app, index++);
        }

        return app;
    }
}