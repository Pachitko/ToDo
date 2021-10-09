using Microsoft.AspNetCore.Authorization;
using Core.Application.Features.Queries.GetJwtToken;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Core.Application.Features.Commands.CreateUser;
using System.Collections.Generic;
using Core.Application.Features.Queries.GetUserById;
using Core.Application.Features.Commands.DeleteUser;
using Core.Application.Features.Queries.GetUsers;
using ToDoApi.Models;
using AutoMapper;
using System;
using Newtonsoft.Json;
using Core.Application.Helpers;
using Core.Application.Services;
using Core.Domain.Entities;
using Core.Application.Extensions;
using System.Linq;
using Microsoft.Net.Http.Headers;
using ToDoApi.ActionConstraints;
using Core.Application.Features.Commands.CreateFullUser;

namespace ToDoApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [Route("api/[controller]")]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IPropertyChecker _propertyChecker;

        public UsersController(IMapper mapper, IPropertyMappingService propertyMappingService, IPropertyChecker propertyChecker)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            _propertyChecker = propertyChecker ?? throw new ArgumentNullException(nameof(propertyChecker));
        }

        [HttpGet(Name = nameof(GetUsersAsync))]
        [HttpHead]
        public async Task<IActionResult> GetUsersAsync([FromQuery] GetUsers.Query query)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<AppUser>(query.OrderBy))
                return BadRequest();

            if (!_propertyChecker.TypeHasProperties<AppUserDto>(query.Fields))
                return BadRequest();

            var response = await Mediator.Send(query);
            if(response.Succeeded)
            {
                var pagedList = response.Value;
               
                var pageMetadata = new
                {
                    totalCount = pagedList.TotalCount,
                    totalPages = pagedList.TotalPages,
                    pageSize = pagedList.PageSize,
                    currentPage = pagedList.CurrentPage
                };

                // When we are requesting an application/json,
                // paging information isn't part of the resource representation,
                // it's a metadata related to that resource
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pageMetadata));

                var links = CreateLinksForUsers(query, pagedList.HasPrevious, pagedList.HasNext);
                var shapedUsers = _mapper.Map<IEnumerable<AppUserDto>>(pagedList).ShapeData(query.Fields);

                var shapedUsersWithLinks = shapedUsers.Select(user =>
                {
                    var userAsDictionary = user as IDictionary<string, object>;
                    var links = CreateLinksForUser((Guid)userAsDictionary["Id"], null);
                    //var resourceWithLinks = _mapper.Map<AppUserDto>(response.Value).ShapeData(fields) as IDictionary<string, object>;
                    userAsDictionary.Add("links", links);
                    return userAsDictionary;
                });

                var linkedUsers =
                    new
                    {
                        value = shapedUsersWithLinks,
                        links
                    };

                return Ok(linkedUsers);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        // application/vnd.todo[.VERSION][.friendly|full][.hateoas]+json
        [HttpGet("{userId}", Name = nameof(GetUserAsync))]
        [Produces("application/json", 
            "application/vnd.todo.hateoas+json", 
            "application/vnd.todo.user.full+json",
            "application/vnd.todo.user.full.hateoas+json", 
            "application/vnd.todo.user.friendly+json",
            "application/vnd.todo.user.friendly.hateoas+json")]
        [ResponseCache(Duration = 30)]
        //[MapToApiVersion("1.1")]
        public async Task<ActionResult<AppUserDto>> GetUserAsync(Guid userId, string fields, [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue parsedMediaType))
                BadRequest();

            if (!_propertyChecker.TypeHasProperties<AppUserDto>(fields))
                return BadRequest();

            var response = await Mediator.Send(new GetUserById.Query(userId));
            if(response.Succeeded)
            {
                bool includeLinks = parsedMediaType.SubTypeWithoutSuffix
                    .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

                IEnumerable<LinkDto> links = new List<LinkDto>();
                if(includeLinks)
                    links = CreateLinksForUser(userId, fields);

                var primaryMediaType = parsedMediaType.SubTypeWithoutSuffix.Value[0..(includeLinks ? ^8 : ^0)];

                if (primaryMediaType == "vnd.todo.user.full")
                {
                    var fullResourceToReturn = _mapper.Map<AppUserFullDto>(response.Value)
                        .ShapeData(fields);

                    if(includeLinks)
                        fullResourceToReturn.TryAdd("links", links);

                    return Ok(fullResourceToReturn);
                }

                var friendlyResourceToReturn = _mapper.Map<AppUserDto>(response.Value)
                    .ShapeData(fields);

                if (includeLinks)
                    friendlyResourceToReturn.TryAdd("links", links);

                return Ok(friendlyResourceToReturn);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpPost(Name = nameof(CreateUserAsync))]
        [AllowAnonymous]
        [RequestHeaderMatchesMediaType("Content-Type", "application/json", "application/vnd.todo.createusercommand+json")]
        [Consumes("application/json", "application/vnd.todo.createusercommand+json")]
        public async Task<ActionResult> CreateUserAsync(CreateUser.Command command)
        {
            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                var userToReturn = _mapper.Map<AppUserDto>(response.Value);
                var links = CreateLinksForUser(userToReturn.Id, null);

                var resourceWithLinks = userToReturn.ShapeData(null);
                resourceWithLinks.TryAdd("links", links);

                //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                return CreatedAtAction(nameof(GetUserAsync), new
                {
                    userId = userToReturn.Id,
                    fields = ""
                }, resourceWithLinks);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpPost(Name = nameof(CreateFullUserAsync))]
        [AllowAnonymous]
        [RequestHeaderMatchesMediaType("Content-Type", "application/vnd.todo.createfullusercommand+json")]
        [Consumes("application/vnd.todo.createfullusercommand+json")]
        public async Task<ActionResult> CreateFullUserAsync(CreateFullUser.Command command)
        {
            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                var userToReturn = _mapper.Map<AppUserFullDto>(response.Value);
                var links = CreateLinksForUser(userToReturn.Id, null);

                var resourceWithLinks = userToReturn.ShapeData(null);
                resourceWithLinks.TryAdd("links", links);

                //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                return CreatedAtAction(nameof(GetUserAsync), new
                {
                    userId = userToReturn.Id,
                    fields = ""
                }, resourceWithLinks);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpDelete("{userId}", Name = nameof(DeleteUserAsync))]
        public async Task<ActionResult> DeleteUserAsync([FromRoute] DeleteUserById.Command command)
        {
            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpGet("token")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<ActionResult> GetTokenAsync(GetJwtToken.Query query)
        {
            var response = await Mediator.Send(query);
            if (response.Succeeded)
            {
                return Ok(new { token = response.Value });
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpOptions]
        public ActionResult GetUsersOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateUser(int id, UpdateProductCommand command)
        //{
        //    if (id != command.Id)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(await Mediator.Send(command));
        //}

        // HATEOAS - Hypermedia as the Engine of Application State
        private IEnumerable<LinkDto> CreateLinksForUser(Guid userId, string fields)
        {
            List<LinkDto> links = new();

            if(string.IsNullOrWhiteSpace(fields))
            {
                links.Add(new(Url.Link(nameof(GetUserAsync), new { userId }), "self", "GET"));
            }
            else
            {
                links.Add(new(Url.Link(nameof(GetUserAsync), new { userId, fields }), "self", "GET"));
            }

            links.Add(new(Url.Link(nameof(DeleteUserAsync), new { userId }), "delete_user", "DELETE"));

            links.Add(new(Url.Link(nameof(TaskListsController.CreateToDoListAsync), new { userId }), "create_taskList_for_user", "POST"));
            
            links.Add(new(Url.Link(nameof(TaskListsController.GetToDoListsAsync), new { userId }), "taskLists", "GET"));

            //links.Add(new(Url.Link(nameof(CreateUserAsync), new { userId }), "create_user", "POST"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForUsers(GetUsers.Query query, bool hasPrevious, bool hasNext)
        {
            List<LinkDto> links = new();

            links.Add(new(CreateResourcePageUri(query, nameof(GetUsersAsync), ResourcePageUriType.Current), "self", "GET"));

            if (hasPrevious)
                links.Add(new(CreateResourcePageUri(query, nameof(GetUsersAsync), ResourcePageUriType.PreviousPage), "previousPage", "GET"));

            if (hasNext)
                links.Add(new(CreateResourcePageUri(query, nameof(GetUsersAsync), ResourcePageUriType.NextPage), "nextPage", "GET"));

            return links;
        }
    }
}