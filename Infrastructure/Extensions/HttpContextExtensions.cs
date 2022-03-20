using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserToken(this HttpContext httpContext)
        {
            return httpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
