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
        public record Query(Guid ToDoListId) : IRequestWrapper<IList<ToDoItem>>;

        public class QueryHandler : IHandlerWrapper<Query, IList<ToDoItem>>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMediator _mediator;

            public QueryHandler(IApplicationDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext;
                _mediator = mediator;
            }

            public async Task<Response<IList<ToDoItem>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetToDoListById.GetToDoListById.Query(request.ToDoListId), cancellationToken);
                if(response.Succeeded)
                {
                    var toDoList = response.Value;
                    if (toDoList is null)
                        return ResponseResult.Ok<IList<ToDoItem>>(null);

                    await _dbContext.Entry(toDoList).Collection(l => l.ToDoItems).LoadAsync(cancellationToken);
                    return ResponseResult.Ok(toDoList.ToDoItems);
                }
                else
                {
                    return ResponseResult.Fail<IList<ToDoItem>>(response.Errors);
                }
            }
        }
    }
}