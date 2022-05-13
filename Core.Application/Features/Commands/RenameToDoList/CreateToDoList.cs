using Core.Application.Features.Queries.GetToDoListById;
using Core.DomainServices.Abstractions;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using FluentValidation;
using AutoMapper;
using MediatR;
using System;

namespace Core.Application.Features.Commands.RenameToDoList
{
    public partial class RenameToDoList
    {
        public record Command(Guid ToDoListId, string NewTitle) : IRequestWrapper<ToDoList>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.NewTitle)
                    .NotEmpty().WithMessage("Title is empty")
                    .MaximumLength(128);
            }
        }

        public class CommandHandler : IHandlerWrapper<RenameToDoList.Command, ToDoList>
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

            public async Task<Response<ToDoList>> Handle(RenameToDoList.Command request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetToDoListById.Query(request.ToDoListId), cancellationToken);
                if (response.Succeeded)
                {
                    var toDoListFromDb = response.Value;

                    _dbContext.ToDoLists.Attach(toDoListFromDb);
                    toDoListFromDb.Title = request.NewTitle;

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    var updatedToDoList = _mapper.Map<ToDoList>(toDoListFromDb);
                    return Response<ToDoList>.Ok(updatedToDoList);
                }
                else
                {
                    return Response<ToDoList>.Fail(response.Errors);
                }
            }
        }
    }
}