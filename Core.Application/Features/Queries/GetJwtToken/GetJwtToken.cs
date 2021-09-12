using Microsoft.AspNetCore.Identity;
using Core.Application.Responses;
using Core.Application.Services;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using FluentValidation;
using MediatR;

namespace Core.Application.Features.Queries.GetJwtToken
{
    public partial class GetJwtToken
	{
		public record Query(string Username, string Password) : IRequestWrapper<string>;

		public class QueryValidator : AbstractValidator<GetJwtToken.Query>
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

		public class QueryHandler : IHandlerWrapper<GetJwtToken.Query, string>
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

			public async Task<Response<string>> Handle(GetJwtToken.Query request, CancellationToken cancellationToken)
			{
				var user = await _userManager.FindByNameAsync(request.Username);
				if (user == null)
				{
					return Response<string>.Ok(null);
				}

				var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					return Response<string>.Ok(await _jwtGenerator.CreateTokenAsync(user));
				}

				return Response<string>.Ok(null);
			}
		}
    }
}