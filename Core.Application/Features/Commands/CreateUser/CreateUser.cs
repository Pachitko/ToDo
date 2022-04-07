﻿using Core.Application.Abstractions;
using Core.Application.Responses;
using Core.Domain.Entities;
using FluentValidation;
using FluentValidation.Validators;
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
            private readonly IMapper _mapper;
            private readonly IEmailConfirmationLinkSender _emailConfirmationLinkSender;

            public CommandHandler(IMapper mapper, UserManager<AppUser> userManager,
                 ILogger<CommandHandler> logger, IEmailConfirmationLinkSender emailConfirmationLinkSender)
            {
                _mapper = mapper;
                _userManager = userManager;
                _logger = logger;
                _emailConfirmationLinkSender = emailConfirmationLinkSender;
            }

            public async Task<Response<AppUser>> Handle(Command request, CancellationToken cancellationToken)
            {
                var newUser = _mapper.Map<AppUser>(request);
                var result = await _userManager.CreateAsync(newUser, request.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account using password.");
                    await _userManager.AddToRoleAsync(newUser, "User");

                    await _emailConfirmationLinkSender.SendConfirmationCodeAsync(newUser);

                    return Response<AppUser>.Ok(newUser);
                }
                else
                {
                    var errors = result.Errors.Select(e => new ResponseError(e.Code, e.Description));

                    return Response<AppUser>.Fail(errors, null);
                }
            }
        }
    }
}