using Microsoft.AspNetCore.Identity;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using System.Linq;
using Core.Application.Services;

namespace Core.Application.Features.Queries.JwtLogin
{
    public partial class JwtTokenGetting
	{
        public class QueryHandler : IHandlerWrapper<JwtTokenGetting.Query, string>
        {
			private readonly UserManager<AppUser> _userManager;
			private readonly SignInManager<AppUser> _signInManager;
			private readonly IJwtGenerator _jwtGenerator;

			public QueryHandler(UserManager<AppUser> userManager, 
				SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
			{
				_userManager = userManager;
				_signInManager = signInManager;
				_jwtGenerator = jwtGenerator;
			}

			public async Task<Response<string>> Handle(JwtTokenGetting.Query request, CancellationToken cancellationToken)
			{
				var validationResult = await new JwtTokenGetting.QueryValidator().ValidateAsync(request, cancellationToken);
				if (!validationResult.IsValid)
					return ResponseResult.Fail<string>(validationResult.Errors.Select(e => new ResponseError(e.PropertyName, e.ErrorMessage)));

				var user = await _userManager.FindByNameAsync(request.Username);
				if (user == null)
				{
					return ResponseResult.Fail<string>(new[] { new ResponseError("", "Username does not exist") });
				}

				var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					return ResponseResult.Ok(await _jwtGenerator.CreateTokenAsync(user));
				}

				return ResponseResult.Fail<string>(new[] { new ResponseError("", "Unauthorized") });
			}
		}
    }
}