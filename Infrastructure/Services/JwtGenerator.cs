using Core.Application.Abstractions;
using Core.Domain.Entities;
using Google.Apis.Logging;
using Infrastructure.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal class JwtGenerator : IJwtGenerator
    {
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<JwtGenerator> _logger;
        private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;

        public JwtGenerator(
            IOptionsSnapshot<JwtOptions> jwtOptions,
            ILogger<JwtGenerator> logger,
            IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory)
        {
            if (jwtOptions is null)
            {
                throw new ArgumentNullException(nameof(jwtOptions));
            }

            _jwtOptions = jwtOptions?.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory ?? throw new ArgumentNullException(nameof(userClaimsPrincipalFactory));
        }

        public async Task<string> CreateTokenAsync(AppUser user)
        {
            _logger.LogDebug("Creating JWT token for user {UserId}", user.Id);

            var claimsPrincipal = await _userClaimsPrincipalFactory.CreateAsync(user);

            SigningCredentials credentials = new(_jwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512Signature);

            var now = DateTime.Now;
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claimsPrincipal.Claims),
                SigningCredentials = credentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddMinutes(_jwtOptions.MinutesExpiration),
            };

            JwtSecurityTokenHandler tokenHandler = new();

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            _logger.LogDebug("JWT token created for user {UserId}", user.Id);
            return tokenString;
        }
    }
}
