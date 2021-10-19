namespace Woa.Api.Startup;

public static class Routing
{
    public static WebApplication UseRoute(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");
    }
}
