﻿using Core.Application.Abstractions.ExternalLogin;
using Core.Application.Responses;
using Core.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Features.Commands.CreateUserWithGoogle
{
    public partial class CreateUserWithExternalLoginProvider
    {
        public record Command(string ProviderName, string Username, string TokenId) : IRequestWrapper<AppUser>;

        public class CommandValidator : AbstractValidator<CreateUserWithExternalLoginProvider.Command>
        {
            public CommandValidator(UserManager<AppUser> userManager)
            {
                RuleFor(u => u.Username)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Username can't be empty")
                    //.Matches(@"^(?=.{4,255}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$").WithMessage("Invalid username")
                    .MustAsync(async (username, _) => !string.IsNullOrEmpty(username) && await userManager.FindByNameAsync(username) == null)
                        .WithMessage("Username already exists");

                RuleFor(u => u.TokenId)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("TokenId can't be empty");
            }
        }

        public class CommandHandler : IHandlerWrapper<Command, AppUser>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly IExternalLoginFactory _externalLoginProviderFactory;

            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor,
                IEmailSender emailSender, ILogger<CommandHandler> logger, IExternalLoginFactory externalLoginProviderFactory)
            {
                _userManager = userManager;
                _httpContextAccessor = httpContextAccessor;
                _emailSender = emailSender;
                _logger = logger;
                _externalLoginProviderFactory = externalLoginProviderFactory;
            }

            public async Task<Response<AppUser>> Handle(Command request, CancellationToken cancellationToken)
            {
                var externalLoginProvider = _externalLoginProviderFactory
                    .GetExternalLoginProvider(_httpContextAccessor.HttpContext, request.ProviderName);
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

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            throw new System.Exception("RequireConfirmedAccount");
                        }

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