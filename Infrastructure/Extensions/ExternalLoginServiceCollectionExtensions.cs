using Core.Application.Abstractions.ExternalLogin;
using Infrastructure.Services.ExternalLogin;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Extensions
{
    public static class ExternalLoginServiceCollectionExtensions
    {
        public static ExternalLoginBuilder AddExternalLogin(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IExternalLoginFactory, ExternalLoginFactory>();
            return new ExternalLoginBuilder(services);
        }
    }
}
