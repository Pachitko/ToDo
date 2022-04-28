using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using ToDoApi;
using System;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(o =>
{
    o.AddServerHeader = false;
});

Startup startup = new(builder.Configuration);
startup.ConfigureServices(builder.Services);

WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.SeedDataAsync();

startup.Configure(app, app.Environment);

app.Run();