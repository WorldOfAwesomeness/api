var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRotes();

app.Run();
