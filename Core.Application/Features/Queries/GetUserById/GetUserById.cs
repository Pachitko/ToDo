using Microsoft.AspNetCore.Identity;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using System;

namespace Core.Application.Features.Queries.GetUserById
{
    public partial class GetUserById
    {
        public record Query(Guid Id) : IRequestWrapper<AppUser>;

        public class QueryHandler : IHandlerWrapper<Query, AppUser>
        {
            private readonly UserManager<AppUser> _userManager;

            public QueryHandler(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Response<AppUser>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userFromDb = await _userManager.FindByIdAsync(request.Id.ToString());
                return ResponseResult.Ok(userFromDb);
            }
        }
    }
}
