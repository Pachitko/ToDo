using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Models;
using Core.Application.Features.Queries.GetToDoItems;
using Core.Application.Features.Commands.CreateToDoItem;
using Core.Application.Features.Queries.GetToDoItemById;
using Infrastructure.Extensions;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/taskLists/{toDoListId}/[controller]")]
    public class TasksController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TasksController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<ToDoItemDto>>> GetToDoItemsAsync(Guid toDoListId)
        {
            var response = await _mediator.Send(new GetToDoItems.Query(toDoListId));
            if (response.Succeeded)
            {
                if (response.Value is null)
                    return NotFound();
                return _mapper.Map<List<ToDoItemDto>>(response.Value);
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }

        [HttpGet("{toDoItemId}", Name = nameof(GetToDoItemAsync))]
        public async Task<ActionResult<ToDoItemDto>> GetToDoItemAsync(Guid toDoListId, Guid toDoItemId)
        {
            var response = await _mediator.Send(new GetToDoItemById.Query(toDoItemId));
            if(response.Succeeded)
            {
                var toDoItem = response.Value;
                if (toDoItem is null)
                    return NotFound();
                return Ok(_mapper.Map<ToDoItemDto>(response.Value));
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateToDoItemAsync(Guid toDoListId, CreateToDoItem.Command command)
        {
            var response = await _mediator.Send(command);
            if (response.Succeeded)
            {
                var toDoItemToReturn = _mapper.Map<ToDoItemDto>(response.Value);
                return CreatedAtAction(nameof(GetToDoItemAsync), new { toDoListId, toDoItemId = toDoItemToReturn.Id }, toDoItemToReturn);
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }
    }
}