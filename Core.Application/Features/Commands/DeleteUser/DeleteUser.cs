using Microsoft.AspNetCore.Identity;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using System;

namespace Core.Application.Features.Commands.DeleteUser
{
    public class DeleteUser
    {
        public record Command(AppUser User) : IRequestWrapper<IdentityResult>;
        
        public class CommandHandler : IHandlerWrapper<Command, IdentityResult>
        {
            private readonly UserManager<AppUser> _userManager;

            public CommandHandler(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Response<IdentityResult>> Handle(Command request, CancellationToken cancellationToken)
            {
                var userFromDb = await _userManager.DeleteAsync(request.User);
                return ResponseResult.Ok(userFromDb);
            }
        }
    }
}
