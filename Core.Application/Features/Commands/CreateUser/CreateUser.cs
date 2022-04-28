using Core.Application.Abstractions;
using Core.Application.Features.Notifications.UserCreated;
using Core.Application.Responses;
using Core.Domain.Entities;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IMapper = AutoMapper.IMapper;

namespace Core.Application.Features.Commands.CreateUser
{
    public partial class CreateUser
    {
        public record Command(string Username, string Email, string Password, string PasswordConfirmation) : IRequestWrapper<AppUser>;
        public class CommandValidator : AbstractValidator<CreateUser.Command>
        {
            public CommandValidator(UserManager<AppUser> userManager)
            {
                RuleFor(u => u.Email)
                    .NotEmpty()
                        .WithMessage("Email can't be empty")
                    .EmailAddress(EmailValidationMode.AspNetCoreCompatible)
                        .WithMessage("Invalid email")
                    .MustAsync(async (email, _) => !string.IsNullOrEmpty(email) && await userManager.FindByEmailAsync(email) == null)
                        .WithMessage("Email already exists");

                RuleFor(u => u.Username)
                    .NotEmpty()
                        .WithMessage("Username can't be empty")
                    .MustAsync(async (username, _) => !string.IsNullOrEmpty(username) && await userManager.FindByNameAsync(username) == null)
                        .WithMessage("Username already exists");

                RuleFor(u => u.Password)
                    .NotEmpty()
                        .WithMessage("Password can't be empty");

                RuleFor(u => u.PasswordConfirmation)
                    .Equal(u => u.Password)
                        .WithMessage("Password mismatch");
            }
        }

        public class CommandHandler : IHandlerWrapper<Command, AppUser>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly ILogger<CommandHandler> _logger;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

            public CommandHandler(IMediator mediator, IMapper mapper, UserManager<AppUser> userManager,
                ILogger<CommandHandler> logger)
            {
                _mediator = mediator;
                _mapper = mapper;
                _userManager = userManager;
                _logger = logger;
            }

            public async Task<Response<AppUser>> Handle(Command request, CancellationToken cancellationToken)
            {
                var newUser = _mapper.Map<AppUser>(request);
                var userCreationResult = await _userManager.CreateAsync(newUser, request.Password);
                if (userCreationResult.Succeeded)
                {
                    _logger.LogInformation("User created a new account using password.");
                    await _mediator.Publish(new UserCreated(newUser), cancellationToken);
                    return Response<AppUser>.Ok(newUser);
                }
                else
                {
                    var errors = userCreationResult.Errors.Select(e => new ResponseError(e.Code, e.Description));
                    return Response<AppUser>.Fail(errors, null);
                }
            }
        }
    }
}