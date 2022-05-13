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
using Core.Application.Features.Commands.RenameToDoList;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskListsController : BaseApiController
    {
        private readonly IMapper _mapper;

        public TaskListsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpHead]
        [HttpGet(Name = nameof(GetToDoListsAsync))]
        public async Task<ActionResult<List<ToDoListDto>>> GetToDoListsAsync([FromRoute] GetToDoLists.Query query)
        {
            var response = await Mediator.Send(query);
            if (response.Succeeded)
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
            if (response.Succeeded)
            {
                return Ok(_mapper.Map<ToDoListDto>(response.Value));
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpPost(Name = nameof(CreateToDoListAsync))]
        public async Task<ActionResult> CreateToDoListAsync(CreateToDoList.Command command)
        {
            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                var toDoListToReturn = _mapper.Map<ToDoListDto>(response.Value);
                return CreatedAtAction(nameof(GetToDoListAsync),
                    new
                    {
                        toDoListId = toDoListToReturn.Id
                    }, toDoListToReturn);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpPost("{toDoListId}", Name = nameof(RenameToDoListAsync))]
        public async Task<ActionResult> RenameToDoListAsync([FromRoute] Guid toDoListId, RenameToDoList.Command command)
        {
            command = command with { ToDoListId = toDoListId };

            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                return Ok(_mapper.Map<ToDoListDto>(response.Value));
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