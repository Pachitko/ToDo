using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using IMapper = AutoMapper.IMapper;
using System.Collections.Generic;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using FluentValidation;
using FluentValidation.Validators;
using System;

namespace Core.Application.Features.Commands.CreateFullUser
{
    public partial class CreateFullUser
    {
        public record Command : CreateUser.CreateUser.Command, IRequestWrapper<AppUser>
        {
            public string MiddleName { get; set; }
            public DateTimeOffset? DateOfBirth { get; set; } // 2000-10-31T21:00:00
            public DateTimeOffset? DateOfDeath { get; set; } 
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(UserManager<AppUser> userManager)
            {
                RuleFor(u => u.Email)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Email can't be empty")
                    .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email")
                    //.MaximumLength(255).WithMessage("Maximum length is 255")
                    .MustAsync(async (email, _) => !string.IsNullOrEmpty(email) && await userManager.FindByEmailAsync(email) == null)
                        .WithMessage("Email already exists");

                RuleFor(u => u.Username)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Email can't be empty")
                    //.Matches(@"^(?=.{4,255}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$").WithMessage("Incorrect username")
                    .MustAsync(async (username, _) => !string.IsNullOrEmpty(username) && await userManager.FindByNameAsync(username) == null)
                        .WithMessage("Username already exists");

                RuleFor(u => u.PhoneNumber)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Phone number can't be empty")
                    .Matches(@"\d{11}").WithMessage("Enter valid phone number: \\d{11}");

                RuleFor(u => u.Password)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Password can't be empty");
                //.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,255}$")
                //.WithMessage("Min 8 / max 255 characters, at least one uppercase letter, one lowercase letter and one number");

                RuleFor(u => u.FirstName)
                    .NotEmpty().WithMessage("First name can't be empty")
                    .MaximumLength(64);

                RuleFor(u => u.MiddleName)
                   //.NotEmpty().WithMessage("Middle name can't be empty")
                   .MaximumLength(64);

                RuleFor(u => u.LastName)
                    .NotEmpty().WithMessage("Last name can't be empty")
                    .MaximumLength(64);

                RuleFor(u => u.DateOfBirth)
                    .LessThan(u => u.DateOfDeath);
            }
        }

        public class CommandHandler : IHandlerWrapper<Command, AppUser>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly IMapper _mapper;

            private readonly HttpContext _httpContext;
            private readonly LinkGenerator _linkGenerator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMapper mapper, UserManager<AppUser> userManager, IHttpContextAccessor accessor,
                LinkGenerator generator, IEmailSender emailSender, ILogger<CommandHandler> logger)
            {
                _mapper = mapper;
                _userManager = userManager;
                _httpContext = accessor.HttpContext;
                _linkGenerator = generator;
                _emailSender = emailSender;
                _logger = logger;
            }

            public async Task<Response<AppUser>> Handle(Command request, CancellationToken cancellationToken)
            {
                var newUser = _mapper.Map<AppUser>(request);
                var result = await _userManager.CreateAsync(newUser, request.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(newUser, "User");
                    //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = _linkGenerator.GetPathByName(_httpContext, "ConfirmEmail", values: new { newUser.Id });

                    await _emailSender.SendEmailAsync(request.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl ?? "")}'>clicking here</a>.");

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