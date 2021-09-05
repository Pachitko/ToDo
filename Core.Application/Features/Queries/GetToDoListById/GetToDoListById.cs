using Microsoft.EntityFrameworkCore;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using System;

namespace Core.Application.Features.Queries.GetToDoListById
{
    public class GetToDoListById
    {
        public record Query(Guid Id) : IRequestWrapper<ToDoList>;

        public class QueryHandler : IHandlerWrapper<Query, ToDoList>
        {
            private readonly IApplicationDbContext _dbContext;

            public QueryHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Response<ToDoList>> Handle(Query request, CancellationToken cancellationToken)
            {
                var toDoListFromDb = await _dbContext.ToDoLists.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
                return ResponseResult.Ok(toDoListFromDb);
            }
        }
    }
}