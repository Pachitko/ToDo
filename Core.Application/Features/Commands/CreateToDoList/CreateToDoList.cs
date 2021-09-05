using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using AutoMapper;
using System;
using MediatR;
using Core.Application.Features.Queries.GetUserById;
using FluentValidation;

namespace Core.Application.Features.Commands.CreateToDoList
{
    public partial class CreateToDoList
    {
        public record Command(string Title, Guid UserId) : IRequestWrapper<ToDoList>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Title is empty")
                    .MaximumLength(128);
            }
        }

        public class CommandHandler : IHandlerWrapper<CreateToDoList.Command, ToDoList>
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

            public async Task<Response<ToDoList>> Handle(CreateToDoList.Command request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetUserById.Query(request.UserId), cancellationToken);
                if (response.Succeeded)
                {
                    var toDoList = response.Value;
                    if (toDoList is null)
                        return ResponseResult.Ok<ToDoList>(null);

                    var newToDoList = _mapper.Map<ToDoList>(request);
                    await _dbContext.ToDoLists.AddAsync(newToDoList, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return ResponseResult.Ok(newToDoList);
                }
                else
                {
                    return ResponseResult.Fail<ToDoList>(response.Errors, null);
                }
            }
        }
    }
}