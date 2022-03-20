using Microsoft.AspNetCore.Identity;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using System;

namespace Core.Application.Features.Queries.GetCurrentUser
{
    public partial class GetCurrentUser
    {
        public record Query() : IRequestWrapper<AppUser>;

        public class QueryHandler : IHandlerWrapper<Query, AppUser>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly UserManager<AppUser> _userManager;

            public QueryHandler(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            public async Task<Response<AppUser>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userFromDb = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                if (userFromDb is null)
                    return Response<AppUser>.Fail();
                else
                    return Response<AppUser>.Ok(userFromDb);
            }
        }
    }
}
