using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Woa.Api.Startup
{

    public static class Routing
    {
        public static WebApplication UseRoute(this WebApplication app)
        {
            app.MapGet("/", () => "Hello World!");
            var services = app.Services.GetServices<IServiceContract>();

            var index = 1;
            foreach (var service in services)
            {
                service.Register(app, Methods.Get, service.RelativePath, index++);
            }

            return app;
        }
    }
}
