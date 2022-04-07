using Microsoft.AspNetCore.Identity;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using FluentValidation;
using Core.Application.Abstractions;

namespace Core.Application.Features.Commands.Login
{
    public partial class Login
	{
		public record Query(string Username, string Password) : IRequestWrapper<LoginResult>;

		public class QueryValidator : AbstractValidator<Login.Query>
		{
			public QueryValidator()
			{
				RuleFor(u => u.Username)
					.NotEmpty().WithMessage("Username is empty");
				//.EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email");

				RuleFor(u => u.Password)
					.NotEmpty().WithMessage("Password is empty");
			}
		}

		public class QueryHandler : IHandlerWrapper<Login.Query, LoginResult>
        {
			private readonly UserManager<AppUser> _userManager;
			private readonly SignInManager<AppUser> _signInManager;
			private readonly IJwtGenerator _jwtGenerator;

			public QueryHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
				IJwtGenerator jwtGenerator)
			{
				_userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
				_signInManager = signInManager ?? throw new System.ArgumentNullException(nameof(signInManager));
				_jwtGenerator = jwtGenerator ?? throw new System.ArgumentNullException(nameof(jwtGenerator));
			}

			public async Task<Response<LoginResult>> Handle(Login.Query request, CancellationToken cancellationToken)
			{
				var user = await _userManager.FindByNameAsync(request.Username);
				if (user == null)
				{
					return Response<LoginResult>.Ok(null);
				}

				SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
				LoginResult loginResult = new()
				{
					Succeeded = result.Succeeded,
					IsLockedOut = result.IsLockedOut,
					IsNotAllowed = result.IsNotAllowed,
					RequiresTwoFactor= result.RequiresTwoFactor
				};

				if (result.Succeeded)
				{
					loginResult.Token = await _jwtGenerator.CreateTokenAsync(user);
				}

				return Response<LoginResult>.Ok(loginResult);
			}
		}
    }
}