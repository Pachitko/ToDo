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

namespace Core.Application.Features.Commands.JwtRegister
{
    public partial class CreateUser
    {
        public record Command : IRequestWrapper<AppUser>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            //public string ConfirmPassword { get; set; }
        }

        public class CommandHandler : IHandlerWrapper<CreateUser.Command, AppUser>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly IMapper _mapper;

            private readonly HttpContext _httpContext;
            private readonly LinkGenerator _linkGenerator;
            private readonly ILogger<CommandHandler> _logger;

            public CommandHandler(IMapper mapper,
                UserManager<AppUser> userManager,
                IHttpContextAccessor accessor,
                LinkGenerator generator,
                IEmailSender emailSender,
                ILogger<CommandHandler> logger)
            {
                _mapper = mapper;
                _userManager = userManager;

                _httpContext = accessor.HttpContext;
                _linkGenerator = generator;
                _emailSender = emailSender;
                _logger = logger;
            }

            public async Task<Response<AppUser>> Handle(CreateUser.Command request, CancellationToken cancellationToken)
            {
                //var validationResult = await new CreateUser.CommandValidator(_userManager).ValidateAsync(request, cancellationToken);
                //if (!validationResult.IsValid)
                //    return ResponseResult.Fail<AppUser>(validationResult.Errors.Select(e => new ResponseError(e.PropertyName, e.ErrorMessage)), null);

                var user = _mapper.Map<AppUser>(request);
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, "User");
                    //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = _linkGenerator.GetPathByName(_httpContext, "ConfirmEmail", values: new { user.Id });

                    await _emailSender.SendEmailAsync(request.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl ?? "")}'>clicking here</a>.");

                    return ResponseResult.Ok(user);
                }
                else
                {
                    List<ResponseError> errors = result.Errors.
                        Select(e => new ResponseError(e.Code, e.Description)).ToList();

                    return ResponseResult.Fail<AppUser>(errors, null);
                }
            }
        }
    }
}