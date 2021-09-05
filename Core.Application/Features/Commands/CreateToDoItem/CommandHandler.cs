using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using System.Linq;
using AutoMapper;
using System;
using Infrastructure.Data;
using FluentValidation;
using MediatR;
using Core.Application.Features.Queries.GetToDoListById;

namespace Core.Application.Features.Commands.CreateToDoItem
{
    public partial class CreateToDoItem
    {
        public class Command : IRequestWrapper<ToDoItem>
        {
            public Guid ToDoListId { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public bool? IsCompleted { get; set; }

            public bool? IsImportant { get; set; }

            public DateTime? DueDate { get; set; }
        }

        public class CommandValidator : AbstractValidator<CreateToDoItem.Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title)
                    .NotEmpty()
                    .MaximumLength(128);

                RuleFor(x => x.Description)
                    .MaximumLength(512);
            }
        }

        public class CommandHandler : IHandlerWrapper<CreateToDoItem.Command, ToDoItem>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public CommandHandler(IApplicationDbContext dbContext, IMapper mapper, IMediator mediator)
            {
                _dbContext = dbContext;
                _mapper = mapper;
                _mediator = mediator;
            }

            public async Task<Response<ToDoItem>> Handle(CreateToDoItem.Command request, CancellationToken cancellationToken)
            {
                //var validationResult = await new CreateToDoItem.CommandValidator().ValidateAsync(request, cancellationToken);
                //if (!validationResult.IsValid)
                //    return ResponseResult.Fail<ToDoItem>(validationResult.Errors.Select(e => new ResponseError(e.PropertyName, e.ErrorMessage)), null);

                var response = await _mediator.Send(new GetToDoListById.Query(request.ToDoListId), cancellationToken);
                if (response.Succeeded)
                {
                    var toDoList = response.Value;
                    if (toDoList is null)
                        return ResponseResult.Ok<ToDoItem>(null);

                    var newToDoItem = _mapper.Map<ToDoItem>(request);
                    await _dbContext.ToDoItems.AddAsync(newToDoItem, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return ResponseResult.Ok(newToDoItem);
                }
                else
                {
                    return ResponseResult.Fail<ToDoItem>(response.Errors, null);
                }
            }
        }
    }
}