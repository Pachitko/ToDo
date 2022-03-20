using Microsoft.EntityFrameworkCore;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using System.Linq;
using MediatR;
using System;

namespace Core.Application.Features.Queries.GetToDoListById
{
    public partial class GetToDoListById
    {
        public record Query(Guid ToDoListId) : IRequestWrapper<ToDoList>;

        public class QueryHandler : IHandlerWrapper<Query, ToDoList>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMediator _mediator;

            public QueryHandler(IApplicationDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task<Response<ToDoList>> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetCurrentUser.GetCurrentUser.Query(), cancellationToken);
                if (response.Succeeded)
                {
                    var toDoListFromDb = _dbContext.ToDoLists
                        .AsNoTracking()
                        .FirstOrDefault(l => l.Id == request.ToDoListId && l.UserId == response.Value.Id);
                    if(toDoListFromDb is null)
                        return Response<ToDoList>.Fail();
                    else
                        return Response<ToDoList>.Ok(toDoListFromDb);
                }
                else
                {
                    return Response<ToDoList>.Fail(response.Errors);
                }
            }
        }
    }
}