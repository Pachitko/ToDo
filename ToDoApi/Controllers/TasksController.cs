using Core.Application.Features.Commands.DeleteToDoItemById;
using Core.Application.Features.Queries.GetToDoItemById;
using Core.Application.Features.Commands.CreateToDoItem;
using Core.Application.Features.Commands.PatchToDoItem;
using Core.Application.Features.Queries.GetToDoItems;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDoApi.Models;
using AutoMapper;
using System;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/taskLists/{toDoListId}/[controller]")]
    public class TasksController : BaseApiController
    {
        private readonly IMapper _mapper;

        public TasksController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<ToDoItemDto>>> GetToDoItemsAsync([FromRoute] GetToDoListItems.Query query)
        {
            var response = await Mediator.Send(query);
            if (response.Succeeded)
            {
                return _mapper.Map<List<ToDoItemDto>>(response.Value);
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpGet("{toDoItemId}", Name = nameof(GetToDoItemAsync))]
        public async Task<ActionResult<ToDoItemDto>> GetToDoItemAsync([FromRoute] GetToDoItemById.Query query)
        {
            var response = await Mediator.Send(query);
            if (response.Succeeded)
            {
                return Ok(_mapper.Map<ToDoItemDto>(response.Value));
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateToDoItemAsync(Guid toDoListId, CreateToDoItem.Command command)
        {
            command = command with { ToDoListId = toDoListId };

            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                var toDoItemToReturn = _mapper.Map<ToDoItemDto>(response.Value);
                return CreatedAtAction(nameof(GetToDoItemAsync),
                    new
                    {
                        toDoListId = command.ToDoListId,
                        toDoItemId = toDoItemToReturn.Id
                    }, toDoItemToReturn
                );
            }
            else
            {
                return ResponseFailed(response);
            }
        }

        [HttpDelete("{toDoItemId}", Name = nameof(DeleteToDoItemAsync))]
        public async Task<ActionResult> DeleteToDoItemAsync([FromRoute] DeleteToDoItemById.Command command)
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

        [HttpPatch("{toDoItemId}", Name = nameof(PatchToDoItemAsync))]
        public async Task<ActionResult> PatchToDoItemAsync(Guid toDoItemId,
            [FromBody] PatchToDoItem.Command command)
        {
            command = command with { ToDoItemId = toDoItemId };

            var response = await Mediator.Send(command);
            if (response.Succeeded)
            {
                var toDoItemToReturn = _mapper.Map<ToDoItemDto>(response.Value);
                return new ObjectResult(toDoItemToReturn);
            }
            else
            {
                return ResponseFailed(response);
            }
        }
    }
}