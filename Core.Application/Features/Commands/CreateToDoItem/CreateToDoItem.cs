using Core.Application.Features.Queries.GetCurrentUser;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using FluentValidation;
using AutoMapper;
using MediatR;
using System;

namespace Core.Application.Features.Commands.CreateToDoItem
{
    public partial class CreateToDoItem
    {
        // todo edit according to the ToDoItem Entity
        public record Command(Guid ToDoListId, string Title, bool? IsCompleted, bool? IsImportant, DateTime ModifiedAt,
            DateTime CreatedAt, DateTime? DueDate, Recurrence Recurrence) : IRequestWrapper<ToDoItem>;
       
        public class CommandValidator : AbstractValidator<CreateToDoItem.Command>
        {
            public CommandValidator()
            {
                // todo: complete validation
                RuleFor(x => x.Title)
                    .NotEmpty()
                    .MaximumLength(128);
            }
        }

        public class CommandHandler : IHandlerWrapper<CreateToDoItem.Command, ToDoItem>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

            public CommandHandler(IApplicationDbContext dbContext, IMapper mapper, IMediator mediator)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<Response<ToDoItem>> Handle(CreateToDoItem.Command request, CancellationToken cancellationToken)
            {
                //var validationResult = await new CreateToDoItem.CommandValidator().ValidateAsync(request, cancellationToken);
                //if (!validationResult.IsValid)
                //    return ResponseResult.Fail<ToDoItem>(validationResult.Errors.Select(e => new ResponseError(e.PropertyName, e.ErrorMessage)), null);

                var response = await _mediator.Send(new GetCurrentUser.Query(), cancellationToken);
                if (response.Succeeded)
                {
                    var userFromDb = response.Value;
                    var newToDoItem = _mapper.Map<ToDoItem>(request);
                    newToDoItem.UserId = userFromDb.Id;
                    newToDoItem.CreatedAt = DateTime.UtcNow;
                    newToDoItem.ModifiedAt = DateTime.UtcNow;

                    await _dbContext.ToDoItems.AddAsync(newToDoItem, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Response<ToDoItem>.Ok(newToDoItem);
                }
                else
                {
                    return Response<ToDoItem>.Fail(response.Errors);
                }
            }
        }
    }
}