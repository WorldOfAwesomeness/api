using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;
using Faslinq;
using Microsoft.AspNetCore;
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
                service.Register(app, Methods.Get, "/Search", index++);
            }

            return app;
        }
    }
}
