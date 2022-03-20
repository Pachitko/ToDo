using Core.Application.Features.Queries.GetCurrentUser;
using Microsoft.AspNetCore.Identity;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using MediatR;
using System;

namespace Core.Application.Features.Commands.DeleteUser
{
    public partial class DeleteUserById
    {
        public record Command() : IRequestWrapper<bool?>;
        
        public class CommandHandler : IHandlerWrapper<Command, bool?>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IMediator _mediator;

            public CommandHandler(UserManager<AppUser> userManager, IMediator mediator)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task<Response<bool?>> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetCurrentUser.Query(), cancellationToken);
                if (response.Succeeded)
                {
                    /*var identityResult = */await _userManager.DeleteAsync(response.Value);
                    return Response<bool?>.Ok(true);
                }
                else
                {
                    return Response<bool?>.Fail(response.Errors, null);
                }
            }
        }
    }
}