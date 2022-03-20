using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Infrastructure.Data;
using Core.Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Core.Application.Abstractions;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            //ILogger logger = loggerFactory.CreateLogger<InfrastructureServicesRegistration>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseSqlServer(configuration.GetConnectionString("MSSQL"),
                        o => o.MigrationsAssembly("Infrastructure"))
                    .UseLoggerFactory(loggerFactory));

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }
    }
}
