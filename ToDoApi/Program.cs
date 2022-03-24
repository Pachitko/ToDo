using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using ToDoApi;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(o =>
{
    o.AddServerHeader = false;
});

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();