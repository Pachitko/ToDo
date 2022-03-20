using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using System;
using MediatR;

namespace Core.Application.Features.Queries.GetToDoItems
{
    public partial class GetToDoListItems
    {
        public record Query(Guid ToDoListId) : IRequestWrapper<IList<ToDoItem>>;

        public class QueryHandler : IHandlerWrapper<Query, IList<ToDoItem>>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMediator _mediator;

            public QueryHandler(IApplicationDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task<Response<IList<ToDoItem>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetToDoListById.GetToDoListById.Query(request.ToDoListId), cancellationToken);
                if(response.Succeeded)
                {
                    var toDoListFromDb = response.Value;
                    _dbContext.Entry(toDoListFromDb).State = EntityState.Unchanged;

                    await _dbContext.Entry(toDoListFromDb)
                        .Collection(x => x.ToDoItems)
                        .LoadAsync(cancellationToken);

                    return Response<IList<ToDoItem>>.Ok(toDoListFromDb.ToDoItems);
                }
                else
                {
                    return Response<IList<ToDoItem>>.Fail(response.Errors);
                }
            }
        }
    }
}