using Core.Application.Abstractions.ExternalLogin;
using System.Collections.Generic;
using System;

namespace Infrastructure.Services.ExternalLogin
{
    internal class ExternalLoginFactoryOptions
    {
        public Dictionary<string, Type> ExternalLoginMap { get; set; } = new(StringComparer.Ordinal);

        public void AddExternalLoginProvider(string externalLoginProviderName, Type externalLoginProviderType)
        {
            ExternalLoginMap[externalLoginProviderName] = externalLoginProviderType;
        }
    }
}