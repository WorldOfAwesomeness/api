global using System;
global using System.Collections.Concurrent;
global using System.Collections.Generic;
global using Faslinq;
global using Microsoft.AspNetCore;
global using Woa.Api.Startup;
using Microsoft.AspNetCore.Builder;

namespace Woa.Api
{

    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceContracts();

            var app = builder.Build();

            app.UseRoute();

            app.Run();
        }
    }
}