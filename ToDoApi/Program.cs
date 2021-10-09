using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Infrastructure.Data;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ToDoApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // For async tests
            //System.Threading.ThreadPool.SetMaxThreads(System.Environment.ProcessorCount, System.Environment.ProcessorCount);

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
                await scope.ServiceProvider.SeedDataAsync();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

                //webBuilder.ConfigureKestrel(o =>
                //{
                //    o.AddServerHeader = false;
                //});
            });
    }
}
