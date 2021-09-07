using Microsoft.AspNetCore.Authorization;
using Core.Application.Features.Queries.JwtLogin;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using System.Linq;
using Core.Application.Features.Commands.JwtRegister;
using System.Collections.Generic;
using Core.Application.Features.Queries.GetUserById;
using Core.Application.Features.Commands.DeleteUser;
using Core.Application.Features.Queries.GetUsers;
using Infrastructure.Extensions;
using ToDoApi.Models;
using AutoMapper;
using System;
using Newtonsoft.Json;
using Core.Application.Helpers;
using Core.Application.Services;
using Core.Domain.Entities;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IPropertyMappingService _propertyMappingService;

        public UsersController(IMapper mapper, IMediator mediator, IPropertyMappingService propertyMappingService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        [HttpGet(Name = nameof(GetUsersAsync))]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetUsersAsync([FromQuery] GetUsers.Query query)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<AppUser>(query.OrderBy))
                return BadRequest();

            var response = await _mediator.Send(query);
            if(response.Succeeded)
            {
                var pageedList = response.Value;
                if (pageedList is null)
                    return NotFound();

                var previousPageLink = pageedList.HasPrevious ?
                    CreateResourcePageUri(query, nameof(GetUsersAsync), ResourcePageUriType.PreviousPage) : null;

                var nextPageLink = pageedList.HasNext ?
                    CreateResourcePageUri(query, nameof(GetUsersAsync), ResourcePageUriType.NextPage) : null;

                var pageMetadata = new
                {
                    totalCount = pageedList.TotalCount,
                    totalPages = pageedList.TotalPages,
                    pageSize = pageedList.PageSize,
                    currentPage = pageedList.CurrentPage,
                    previousPageLink,
                    nextPageLink,
                };

                // When we are requesting an application/json,
                // paging information isn't part of the resource representation,
                // it's a metadata related to that resource
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pageMetadata));

                return Ok(_mapper.Map<IEnumerable<AppUserDto>>(response.Value));
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }

        [HttpGet("{userId}", Name = nameof(GetUserAsync))]
        public async Task<ActionResult<AppUserDto>> GetUserAsync(Guid userId)
        {
            var response = await _mediator.Send(new GetUserById.Query(userId));
            if(response.Succeeded)
            {
                var user = response.Value;
                if (user is null)
                    return NotFound();
                return Ok(_mapper.Map<AppUserDto>(response.Value));
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
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
        public async Task<ActionResult> GetTokenAsync(GetJwtToken.Query query)
        {
            var response = await _mediator.Send(query);  
            if (response.Succeeded)
            {
                if (response.Value is null)
                    return NotFound();
                return Ok(new { token = response.Value });
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> CreateUserAsync(CreateUser.Command command)
        {
            var response = await _mediator.Send(command);
            if (response.Succeeded)
            {
                var userToReturn = _mapper.Map<AppUserDto>(response.Value);
                //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                return CreatedAtAction(nameof(GetUserAsync), new { userId = userToReturn.Id }, userToReturn);
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(Guid userId)
        {
            var response = await _mediator.Send(new GetUserById.Query(userId));
            if (response.Succeeded)
            {
                if (response.Value is null)
                    return NotFound();
                await _mediator.Send(new DeleteUser.Command(response.Value));
                return NoContent();
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }

        [HttpOptions]
        public ActionResult GetUsersOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }
    }
}