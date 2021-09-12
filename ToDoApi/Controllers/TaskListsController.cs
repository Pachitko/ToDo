using Core.Application.Features.Commands.DeleteToDoListById;
using Core.Application.Features.Queries.GetToDoListById;
using Core.Application.Features.Commands.CreateToDoList;
using Core.Application.Features.Queries.GetToDoLists;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoApi.Models;
using AutoMapper;
using System;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class TaskListsController : BaseApiController
    {
        private readonly IMapper _mapper;

        public TaskListsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetToDoListsAsync))]
        [HttpHead]
        public async Task<ActionResult<List<ToDoListDto>>> GetToDoListsAsync([FromRoute] GetToDoLists.Query query)
        {
            var response = await Mediator.Send(query);
            if(response.Succeeded)
            {
                return _mapper.Map<List<ToDoListDto>>(response.Value);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpGet("{toDoListId}", Name = nameof(GetToDoListAsync))]
        public async Task<ActionResult<ToDoListDto>> GetToDoListAsync([FromRoute] GetToDoListById.Query query)
        {
            var response = await Mediator.Send(query);
            if(response.Succeeded)  
            {
                return Ok(_mapper.Map<ToDoListDto>(response.Value));
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpPost(Name = nameof(CreateToDoListAsync))]
        public async Task<ActionResult> CreateToDoListAsync(Guid userId, CreateToDoList.Command command)
        {
            command.UserId = userId;

            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                var toDoListToReturn = _mapper.Map<ToDoListDto>(response.Value);
                return CreatedAtAction(nameof(GetToDoListAsync), new { userId, toDoListId = toDoListToReturn.Id }, toDoListToReturn);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpDelete("{toDoListId}", Name = nameof(DeleteToDoListAsync))]
        public async Task<ActionResult> DeleteToDoListAsync([FromRoute] DeleteToDoListById.Command command)
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
    }
}