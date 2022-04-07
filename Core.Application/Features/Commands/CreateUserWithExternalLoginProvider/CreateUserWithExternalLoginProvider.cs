using Core.Application.Abstractions;
using Core.Application.Abstractions.ExternalLogin;
using Core.Application.Responses;
using Core.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Features.Commands.CreateUserWithExternalLoginProvider
{
    public partial class CreateUserWithExternalLoginProvider
    {
        public record Command(string ProviderName, string Username, string TokenId) : IRequestWrapper<AppUser>;

        public class CommandValidator : AbstractValidator<CreateUserWithExternalLoginProvider.Command>
        {
            public CommandValidator(UserManager<AppUser> userManager)
            {
                RuleFor(u => u.Username)
                    .NotEmpty()
                        .WithMessage("Username can't be empty")
                    .MustAsync(async (username, _) => !string.IsNullOrEmpty(username) && await userManager.FindByNameAsync(username) == null)
                        .WithMessage("Username already exists");

                RuleFor(u => u.TokenId)
                    .NotEmpty()
                        .WithMessage("TokenId can't be empty");
            }
        }

        public class CommandHandler : IHandlerWrapper<Command, AppUser>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IExternalLoginFactory _externalLoginProviderFactory;
            private readonly IEmailConfirmationLinkSender _emailConfirmationLinkSender;
            private readonly ILogger<CommandHandler> _logger;
            private readonly HttpContext _httpContext;

            public CommandHandler(UserManager<AppUser> userManager, ILogger<CommandHandler> logger,
                IHttpContextAccessor accessor,
                IExternalLoginFactory externalLoginProviderFactory,
                IEmailConfirmationLinkSender emailConfirmationLinkSender)
            {
                _userManager = userManager;
                _logger = logger;
                _externalLoginProviderFactory = externalLoginProviderFactory;
                _emailConfirmationLinkSender = emailConfirmationLinkSender;
                _httpContext = accessor.HttpContext;
            }

            public async Task<Response<AppUser>> Handle(Command request, CancellationToken cancellationToken)
            {
                var externalLoginProvider = _externalLoginProviderFactory
                    .GetExternalLoginProvider(_httpContext, request.ProviderName);
                var payload = await externalLoginProvider.GetPayloadAsync(request.TokenId);

                var newUser = new AppUser()
                {
                    Email = payload.Email,
                    UserName = request.Username
                };

                var createUserResult = await _userManager.CreateAsync(newUser);
                if (createUserResult.Succeeded)
                {
                    UserLoginInfo userLoginInfo = new(request.ProviderName, payload.Subject, request.ProviderName);

                    var addLoginResult = await _userManager.AddLoginAsync(newUser, userLoginInfo);
                    if (addLoginResult.Succeeded)
                    {
                        _logger.LogInformation($"User created an account using {userLoginInfo.LoginProvider} provider.");
                        await _userManager.AddToRoleAsync(newUser, "User");

                        await _emailConfirmationLinkSender.SendConfirmationCodeAsync(newUser);

                        return Response<AppUser>.Ok(newUser);
                    }
                    else
                    {
                        var errors = addLoginResult.Errors.Select(e => new ResponseError(e.Code, e.Description));
                        return Response<AppUser>.Fail(errors, null);
                    }
                }
                else
                {
                    var errors = createUserResult.Errors.Select(e => new ResponseError(e.Code, e.Description));
                    return Response<AppUser>.Fail(errors, null);
                }
            }
        }
    }
}