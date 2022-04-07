using Core.Application.Abstractions.ExternalLogin;
using Infrastructure.Services.ExternalLogin;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Infrastructure.Extensions
{
    public class ExternalLoginBuilder
    {
        private readonly IServiceCollection _services;

        public ExternalLoginBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ExternalLoginBuilder AddExternalLoginProvider<TOptions, TProvider>(string externalLoginProviderName, Action<TOptions> configureOptions)
            where TOptions : ExternalLoginOptions, new()
            where TProvider : class, IExternalLoginProvider
        {
            _services.Configure<ExternalLoginFactoryOptions>(o =>
            {
                o.AddExternalLoginProvider(externalLoginProviderName, typeof(TProvider));
            });
            
            if (configureOptions != null)
            {
                _services.Configure(externalLoginProviderName, configureOptions);
            }

            _services.AddTransient<TProvider>();
            return this;
        }
    }
}