using Core.Application.Features.Commands.Login;
using Core.Application.Features.Commands.LoginWithExternalProvider;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace ToDoApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly IOptionsMonitor<JwtBearerOptions> _jwtBearerOptionsMonitor;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IOptionsMonitor<JwtBearerOptions> jwtBearerOptionsMonitor, ILogger<AuthController> logger)
        {
            _jwtBearerOptionsMonitor = jwtBearerOptionsMonitor;
            _logger = logger;
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> Login([FromBody] Login.Query query)
        {
            var response = await Mediator.Send(query);
            if (response.Succeeded)
            {
                return Ok(response.Value);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpPost("external")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> LoginWithExternalProvider([FromBody] LoginWithExternalProvider.Command command)
        {
            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                return Ok(response.Value);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpGet("verifyToken", Name = nameof(VerifyToken))]
        [Authorize]
        public ActionResult<bool> VerifyToken()
        {
            var accessToken = HttpContext.Request.Headers.Authorization.ToString()["Bearer ".Length..];

            try
            {
                var options = _jwtBearerOptionsMonitor.Get(JwtBearerDefaults.AuthenticationScheme);

                foreach (var validator in options.SecurityTokenValidators)
                {
                    validator.ValidateToken(accessToken, options.TokenValidationParameters, out var validToken);
                    JwtSecurityToken validJwt = validToken as JwtSecurityToken;

                    if (validJwt is null)
                    {
                        return false;
                    }
                }
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException e)
            {
                _logger.LogError(e, "Token verification failed");
                return false;
            }

            return true;
        }

        [HttpOptions("login")]
        public ActionResult GetUserTokenOptions()
        {
            Response.Headers.Remove("Allow");
            Response.Headers.Add("Allow", "GET");
            return Ok();
        }

    }
}
