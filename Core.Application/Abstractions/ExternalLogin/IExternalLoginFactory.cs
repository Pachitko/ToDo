using Microsoft.AspNetCore.Http;

namespace Core.Application.Abstractions.ExternalLogin
{
    public interface IExternalLoginFactory
    {
        IExternalLoginProvider GetExternalLoginProvider(HttpContext context, string externalLoginProviderName);
    }
}