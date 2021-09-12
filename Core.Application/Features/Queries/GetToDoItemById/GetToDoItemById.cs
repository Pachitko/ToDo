using Microsoft.EntityFrameworkCore;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using MediatR;
using System;

namespace Core.Application.Features.Queries.GetToDoItemById
{
    public partial class GetToDoItemById
    {
        public record Query(Guid UserId, Guid ToDoListId, Guid ToDoItemId) : IRequestWrapper<ToDoItem>;

        public class QueryHandler : IHandlerWrapper<Query, ToDoItem>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMediator _mediator;

            public QueryHandler(IApplicationDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task<Response<ToDoItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetToDoListById.GetToDoListById.Query(request.UserId, request.ToDoListId), cancellationToken);
                if(response.Succeeded)
                {
                    var toDoItemFromDb = await _dbContext.ToDoItems
                        .AsNoTracking()
                        .SingleOrDefaultAsync(t => t.Id == request.ToDoItemId, cancellationToken: cancellationToken);

                    if(toDoItemFromDb is null)
                        return Response<ToDoItem>.Fail();
                    else
                        return Response<ToDoItem>.Ok(toDoItemFromDb);
                }
                else
                {
                    return Response<ToDoItem>.Fail(response.Errors);
                }
            }
        }
    }
}