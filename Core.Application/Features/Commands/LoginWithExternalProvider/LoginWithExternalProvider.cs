using Core.Application.Abstractions;
using Core.Application.Abstractions.ExternalLogin;
using Core.Application.Responses;
using Core.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Features.Commands.LoginWithExternalProvider
{
    public partial class LoginWithExternalProvider
    {
        public record Command(string ProviderName, string TokenId) : IRequestWrapper<LoginResult>;

        public class QueryValidator : AbstractValidator<LoginWithExternalProvider.Command>
        {
            public QueryValidator()
            {
                RuleFor(u => u.TokenId)
                    .NotEmpty().WithMessage("TokenId is empty");
            }
        }

        public class QueryHandler : IHandlerWrapper<LoginWithExternalProvider.Command, LoginResult>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly ILogger<LoginWithExternalProvider.QueryHandler> _logger;
            private readonly IExternalLoginFactory _externalLoginProviderFactory;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public QueryHandler(UserManager<AppUser> userManager,
                IJwtGenerator jwtGenerator, ILogger<LoginWithExternalProvider.QueryHandler> logger,
                IExternalLoginFactory externalLoginProviderFactory, IHttpContextAccessor httpContextAccessor)
            {
                _userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
                _jwtGenerator = jwtGenerator ?? throw new System.ArgumentNullException(nameof(jwtGenerator));
                _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
                _externalLoginProviderFactory = externalLoginProviderFactory ?? throw new System.ArgumentNullException(nameof(externalLoginProviderFactory));
                _httpContextAccessor = httpContextAccessor ?? throw new System.ArgumentNullException(nameof(httpContextAccessor));
            }

            public async Task<Response<LoginResult>> Handle(LoginWithExternalProvider.Command request, CancellationToken cancellationToken)
            {
                var externalLoginProvider = _externalLoginProviderFactory
                    .GetExternalLoginProvider(_httpContextAccessor.HttpContext, request.ProviderName);
                var payload = await externalLoginProvider.GetPayloadAsync(request.TokenId);

                AppUser user = await _userManager.FindByLoginAsync(request.ProviderName, payload.Subject);
                if (user == null)
                {
                    return Response<LoginResult>.Fail(null);
                }

                LoginResult loginResult = new()
                {
                    IsLockedOut = await _userManager.IsLockedOutAsync(user),
                    IsNotAllowed = await _userManager.IsEmailConfirmedAsync(user),
                    RequiresTwoFactor = false
                };

                loginResult.Succeeded = !(loginResult.IsLockedOut || loginResult.IsNotAllowed || loginResult.RequiresTwoFactor);

                if (loginResult.Succeeded)
                {
                    loginResult.Token = await _jwtGenerator.CreateTokenAsync(user);
                }

                return Response<LoginResult>.Ok(loginResult);
            }
        }
    }
}