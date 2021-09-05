using Microsoft.EntityFrameworkCore;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using System;

namespace Core.Application.Features.Queries.GetToDoItemById
{
    public partial class GetToDoItemById
    {
        public record Query(Guid Id) : IRequestWrapper<ToDoItem>;

        public class QueryHandler : IHandlerWrapper<Query, ToDoItem>
        {
            private readonly IApplicationDbContext _dbContext;

            public QueryHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Response<ToDoItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                var toDoItemFromDb = await _dbContext.ToDoItems
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);
                return ResponseResult.Ok(toDoItemFromDb);
            }
        }
    }
}