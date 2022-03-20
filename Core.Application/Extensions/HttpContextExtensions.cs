using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using System;

namespace Infrastructure.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool TryGetUserToken(this HttpContext httpContext, out Guid userToken)
        {
            return Guid.TryParse(httpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out userToken);
        }
    }
}
