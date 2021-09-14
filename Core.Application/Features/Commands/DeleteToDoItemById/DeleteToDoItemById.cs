using Core.Application.Features.Queries.GetToDoItemById;
using System.Collections.Generic;
using Core.Application.Responses;
using System.Threading.Tasks;
using Infrastructure.Data;
using System.Threading;
using MediatR;
using System;

namespace Core.Application.Features.Commands.DeleteToDoItemById
{
    public partial class DeleteToDoItemById
    {
        public record Command(Guid UserId, Guid ToDoListId, Guid ToDoItemId) : IRequestWrapper<bool?>;

        public class CommandHandler : IHandlerWrapper<Command, bool?>
        {
            private readonly IMediator _mediator;
            private readonly IApplicationDbContext _dbContext;

            public CommandHandler(IMediator mediator, IApplicationDbContext dbContext)
            {
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<Response<bool?>> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetToDoItemById.Query(request.UserId, request.ToDoListId, request.ToDoItemId), cancellationToken);
                if (response.Succeeded)
                {
                    _dbContext.ToDoItems.Remove(response.Value);
                    await _dbContext.SaveChangesAsync();
                    return Response<bool?>.Ok(true);
                }
                else
                {
                    return Response<bool?>.Fail(response.Errors, null);
                }
            }
        }
    }
}