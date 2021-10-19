using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Faslinq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;

namespace Woa.Api.Startup
{

    public static class Routing
    {
        public static WebApplication UseRoute(WebApplication app)
        {
            app.MapGet("/", () => "Hello World!");

            return app;
        }
    }
}