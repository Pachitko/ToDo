using Core.Application.Features.Queries.GetUserById;
using Microsoft.AspNetCore.Identity;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using FluentValidation;
using System.Threading;
using MediatR;
using System;

namespace Core.Application.Features.Commands.ConfirmEmail
{
    public partial class ConfirmEmail
    {
        public record Command(Guid UserId, string Code) : IRequestWrapper<bool?>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.UserId).NotEmpty();
                RuleFor(c => c.Code).NotEmpty();
            }
        }

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
                var userResponse = await _mediator.Send(new GetUserById.Query(request.UserId), cancellationToken);
                if (!userResponse.Succeeded)
                {
                    return Response<bool?>.Fail(userResponse.Errors, null);
                }

                var confirmationResult = _userManager.ConfirmEmailAsync(userResponse.Value, request.Code);
                if (confirmationResult.IsCompletedSuccessfully)
                {
                    return Response<bool?>.Ok(true);
                }
                else
                {
                    return Response<bool?>.Fail(null, false);
                }
            }
        }
    }
}