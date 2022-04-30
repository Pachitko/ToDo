using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Infrastructure.Data;
using ToDoApi;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.WebHost.ConfigureKestrel(o =>
{
    o.AddServerHeader = false;
});

builder.Host.UseSerilog();

Startup startup = new(builder.Configuration);
startup.ConfigureServices(builder.Services);

WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.SeedDataAsync();

startup.Configure(app, app.Environment);

await app.RunAsync();