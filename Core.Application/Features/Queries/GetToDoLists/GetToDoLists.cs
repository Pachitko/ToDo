using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;

namespace Core.Application.Features.Queries.GetToDoLists
{
    public partial class GetToDoLists
    {
        public record Query : IRequestWrapper<IList<ToDoList>>;

        public class QueryHandler : IHandlerWrapper<Query, IList<ToDoList>>
        {
            private readonly IApplicationDbContext _dbContext;

            public QueryHandler(IApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Response<IList<ToDoList>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var toDoListsFromDb = await _dbContext.ToDoLists.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
                return ResponseResult.Ok(toDoListsFromDb as IList<ToDoList>);
            }
        }
    }
}