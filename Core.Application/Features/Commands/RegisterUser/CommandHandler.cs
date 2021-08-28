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
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Core.Application.Features.Commands.RegisterUser
{
    public partial class RegisterUser
    {
        public class CommandHandler : IHandlerWrapper<RegisterUser.Command, IdentityResult>
        {
            private readonly SignInManager<AppUser> _signInManager;
            private readonly UserManager<AppUser> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly IMapper _mapper;

            private readonly HttpContext _httpContext;
            private readonly LinkGenerator _linkGenerator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMapper mapper,
                SignInManager<AppUser> signInManager,
                UserManager<AppUser> userManager,
                IHttpContextAccessor accessor,
                LinkGenerator generator,
                IEmailSender emailSender,
                ILogger<CommandHandler> logger)
            {
                _mapper = mapper;
                _signInManager = signInManager;
                _userManager = userManager;

                _httpContext = accessor.HttpContext;
                _linkGenerator = generator;
                _emailSender = emailSender;
                _logger = logger;
            }

            public async Task<Response<IdentityResult>> Handle(RegisterUser.Command request, CancellationToken cancellationToken)
            {
                var validator = new RegisterUser.CommandValidator(_userManager);
                var validationResult = await validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                    return ResponseResult.Fail(validationResult.Errors.Select(e => new ResponseError(e.PropertyName, e.ErrorMessage)),
                        IdentityResult.Failed());

                var user = _mapper.Map<AppUser>(request);
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, "User");
                    //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = _linkGenerator.GetPathByPage(
                        _httpContext,
                        "/Account/ConfirmEmail",
                        handler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl = request.ReturnUrl });

                    await _emailSender.SendEmailAsync(request.Email, "Confirm your email",
                        $"Please confirm your account by clicking <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>here</a>.");

                    if (!_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }

                    return ResponseResult.Ok(result);
                }
                else
                {
                    List<ResponseError> errors = result.Errors.
                        Select(e => new ResponseError(e.Code, e.Description)).ToList();

                    return ResponseResult.Fail(errors, result);
                }
            }
        }
    }
}