﻿using System.IdentityModel.Tokens.Jwt;
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
        private readonly JwtOptions _jwtOptions;
        private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;

		public JwtGenerator(IOptionsSnapshot<JwtOptions> jwtOptions, 
			IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory)
		{
            if (jwtOptions is null)
            {
                throw new ArgumentNullException(nameof(jwtOptions));
            }

            _jwtOptions = jwtOptions?.Value;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory ?? throw new ArgumentNullException(nameof(userClaimsPrincipalFactory));
		}

		public async Task<string> CreateTokenAsync(AppUser user)
		{
			// Register own factory: AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>()
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

			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
