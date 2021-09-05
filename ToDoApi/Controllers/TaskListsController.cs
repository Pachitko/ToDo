using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Models;
using System;
using Core.Application.Features.Queries.GetToDoLists;
using Core.Application.Features.Queries.GetToDoListById;
using Core.Application.Features.Commands.CreateToDoList;
using Infrastructure.Extensions;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskListsController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public TaskListsController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<List<ToDoListDto>>> GetToDoListsAsync()
        {
            var response = await _mediator.Send(new GetToDoLists.Query());
            if(response.Succeeded)
            {
                if (response.Value is null)
                    return NotFound();
                return _mapper.Map<List<ToDoListDto>>(response.Value);
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }

        [HttpGet("{toDoListId}", Name = nameof(GetToDoListAsync))]
        public async Task<ActionResult<ToDoListDto>> GetToDoListAsync(Guid toDoListId)
        {
            var response = await _mediator.Send(new GetToDoListById.Query(toDoListId));
            if(response.Succeeded)
            {
                var toDoList = response.Value;
                if (toDoList is null)
                    return NotFound();
                return Ok(_mapper.Map<ToDoListDto>(response.Value));
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateToDoListAsync(CreateToDoList.Command command)
        {
            var response = await _mediator.Send(command);
            if (response.Succeeded)
            {
                if (response.Value is null)
                    return NotFound();
                var toDoListToReturn = _mapper.Map<ToDoListDto>(response.Value);
                return CreatedAtAction(nameof(GetToDoListAsync), new { toDoListId = toDoListToReturn.Id }, toDoListToReturn);
            }
            else
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
        }
    }
}