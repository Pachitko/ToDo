using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.Application.Abstractions.ExternalLogin
{
    public interface IExternalLoginFactory
    {
        IExternalLoginProvider GetExternalLoginProvider(HttpContext context, string externalLoginProviderName);
    }
}