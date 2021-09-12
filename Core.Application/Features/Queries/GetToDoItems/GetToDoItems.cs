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
    public partial class GetToDoItems
    {
        public record Query(Guid UserId, Guid ToDoListId) : IRequestWrapper<IList<ToDoItem>>;

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
                var response = await _mediator.Send(new GetToDoListById.GetToDoListById.Query(request.UserId, request.ToDoListId), cancellationToken);
                if(response.Succeeded)
                {
                    _dbContext.Entry(response.Value).State = EntityState.Unchanged;
                    await _dbContext.Entry(response.Value).Collection(x => x.ToDoItems).LoadAsync(cancellationToken);
                    return Response<IList<ToDoItem>>.Ok(response.Value.ToDoItems);
                }
                else
                {
                    return Response<IList<ToDoItem>>.Fail(response.Errors);
                }
            }
        }
    }
}