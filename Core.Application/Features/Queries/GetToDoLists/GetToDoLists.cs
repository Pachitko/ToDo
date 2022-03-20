using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using System;
using MediatR;

namespace Core.Application.Features.Queries.GetToDoLists
{
    public partial class GetToDoLists
    {
        public record Query() : IRequestWrapper<IList<ToDoList>>;

        public class QueryHandler : IHandlerWrapper<Query, IList<ToDoList>>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMediator _mediator;

            public QueryHandler(IApplicationDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task<Response<IList<ToDoList>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetCurrentUser.GetCurrentUser.Query(), cancellationToken);
                if(response.Succeeded)
                {
                    var userFromDb = response.Value;
                    _dbContext.Entry(userFromDb).State = EntityState.Unchanged;
                    await _dbContext.Entry(userFromDb).Collection(user => user.ToDoLists).LoadAsync(cancellationToken);
                    return Response<IList<ToDoList>>.Ok(userFromDb.ToDoLists);
                }
                else
                {
                    return Response<IList<ToDoList>>.Fail(response.Errors);
                }
            }
        }
    }
}