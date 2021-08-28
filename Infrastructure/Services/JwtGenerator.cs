using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using System;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Core.Application.Services;
using Core.Domain.Entities;

namespace Infrastructure.Services
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly UserManager<AppUser> _userManager;
		private readonly JwtOptions _jwtOptions;

		public JwtGenerator(UserManager<AppUser> userManager, IOptionsSnapshot<JwtOptions> jwtOptions)
		{
            _userManager = userManager;
			_jwtOptions = jwtOptions.Value;

		}

		public async Task<string> CreateTokenAsync(AppUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
			};

			claims
				.AddRange((await _userManager.GetRolesAsync(user))
				.Select(roleName => new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName)));

			SigningCredentials credentials = new(_jwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512Signature);

			var now = DateTime.Now;
			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Subject = new ClaimsIdentity(claims),
				SigningCredentials = credentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddDays(7),
			};

			JwtSecurityTokenHandler tokenHandler = new();

			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
