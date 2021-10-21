
var builder = WebApplication.CreateBuilder(args);
builder.AddMarkdownServer();
builder.AddServiceContracts();

var app = builder.Build();

app.UseMarkdownServer();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRoute();

app.Run();
