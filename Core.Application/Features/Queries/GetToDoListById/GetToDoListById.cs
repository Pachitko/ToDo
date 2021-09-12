using Microsoft.EntityFrameworkCore;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using System;
using MediatR;

namespace Core.Application.Features.Queries.GetToDoListById
{
    public class GetToDoListById
    {
        public record Query(Guid UserId, Guid ToDoListId) : IRequestWrapper<ToDoList>;

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
                var response = await _mediator.Send(new GetUserById.GetUserById.Query(request.UserId), cancellationToken);
                if (response.Succeeded)
                {
                    var toDoListFromDb = await _dbContext.ToDoLists
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == request.ToDoListId, cancellationToken: cancellationToken);
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