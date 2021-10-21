using Microsoft.Extensions.Options;
using Woa.MarkdownServer;

var builder = WebApplication.CreateBuilder(args);
builder.AddMarkdownServer();
builder.Services.AddHttpContextAccessor();
//builder.Services.AddOptions();

var app = builder.Build();

app.UseMarkdownServer();

app.Map("/", Index);

app.Run();

async Task Index(HttpContext context)
    => await new MarkdownResult("# Hello World!").ExecuteAsync(context);


