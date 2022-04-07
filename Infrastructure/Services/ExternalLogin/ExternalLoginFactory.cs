using Core.Application.Abstractions.ExternalLogin;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.ExternalLogin
{
    internal class ExternalLoginFactory : IExternalLoginFactory
    {
        private readonly IOptions<ExternalLoginFactoryOptions> _options;

        public ExternalLoginFactory(IOptions<ExternalLoginFactoryOptions> options)
        {
            _options = options;
        }

        public IExternalLoginProvider GetExternalLoginProvider(HttpContext context, string externalLoginProviderName)
        {
            if (_options.Value.ExternalLoginMap.TryGetValue(externalLoginProviderName, out var providerType))
            {
                var provider = (context.RequestServices.GetService(providerType) ??
                    ActivatorUtilities.CreateInstance(context.RequestServices, providerType))
                    as IExternalLoginProvider;

                return provider;
            }
            return null;
        }
    }
}
