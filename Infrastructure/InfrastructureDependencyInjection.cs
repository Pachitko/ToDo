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
using Infrastructure.Extensions;
using Infrastructure.Services.ExternalLogin.Google;
using System.Collections.Generic;

namespace Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddFilter(l => l == LogLevel.None);
            });

            var connectionString = configuration.GetConnectionString("DBCS");
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention()
                    .UseLoggerFactory(loggerFactory));

            services.AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
            })
                .AddDefaultTokenProviders()
                .AddSignInManager<SignInManager<AppUser>>()
                .AddRoles<AppRole>()
                .AddRoleManager<RoleManager<AppRole>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();

            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailConfirmationLinkSender, EmailSender>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            services.AddExternalLogin()
                .AddExternalLoginProvider<GoogleOptions, GoogleLoginProvider>(GoogleLoginDefaults.ProviderName, o =>
                {
                    o.Audience = new List<string>() { configuration.GetSection("ExternalLogin:Google:ClientId").Value };
                    o.ClientSecret = configuration.GetSection("ExternalLogin:Google:ClientSecret").Value;
                });

            return services;
        }
    }
}
