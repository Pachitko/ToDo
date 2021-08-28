using Microsoft.AspNetCore.Authorization;
using Core.Application.Features.Queries.JwtLogin;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
using Core.Application.Features.Commands.JwtRegister;
using Core.Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AutoMapper;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _dbContext;

        public UsersController(IMapper mapper, IMediator mediator, ApplicationDbContext applicationDbContext)
        {
            _mapper = mapper;
            _mediator = mediator;
            _dbContext = applicationDbContext;
        }

        [HttpGet]
        [HttpHead]
        public async Task<List<UserDto>> GetUsersAsync()
        {
            return _mapper.Map<List<UserDto>>(await _dbContext.Users.ToListAsync());
        }

        [HttpGet("{userId}", Name = nameof(GetUserAsync))]
        public async Task<ActionResult<AppUser>> GetUserAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            return user is null ? NotFound() : Ok(_mapper.Map<UserDto>(user));
        }

        [HttpGet("me")]
        public ActionResult GetMe()
        {
            return Ok(new
            {
                User.Identity.Name,
                User.Identity.AuthenticationType,
                User.Identity.IsAuthenticated,
                Calims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpGet("token")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> GetTokenAsync(JwtTokenGetting.Query query)
        {
            var loginResult = await _mediator.Send(query);  

            if (loginResult.Succeeded)
            {
                return Ok(new { token = loginResult.Value });
            }
            else
            {
                foreach (var error in loginResult.Errors)
                    ModelState.AddModelError("Errors", error.Description);

                return BadRequest(ModelState);

                //return NotFound(new 
                //{
                //    errors = ModelState[""].Errors.Select(e => e.ErrorMessage) 
                //});
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> CreateUserAsync(CreateUser.Command command)
        {
            var result = await _mediator.Send(command);
            if (result.Succeeded)
            {
                var userToReturn = _mapper.Map<UserDto>(result.Value);
                //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                return CreatedAtAction(nameof(GetUserAsync), new { userId = userToReturn.Id }, userToReturn);
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("errors", error.Description);
              
                return BadRequest(ModelState);
                //return BadRequest(new
                //{
                //    errors = ModelState[""].Errors.Select(e => e.ErrorMessage)
                //});
            }
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteProductForUser(int userId)
        {
            var userFromDb = await _dbContext.Users.FindAsync(userId);
            if (userFromDb is null)
                return NotFound();

            _dbContext.Users.Remove(userFromDb);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpOptions]
        public ActionResult GetUsersOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }
    }
}
