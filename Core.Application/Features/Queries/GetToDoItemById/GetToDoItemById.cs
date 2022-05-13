using Core.Application.Features.Queries.GetCurrentUser;
using Microsoft.EntityFrameworkCore;
using Core.DomainServices.Abstractions;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using MediatR;
using System;

namespace Core.Application.Features.Queries.GetToDoItemById
{
    public partial class GetToDoItemById
    {
        public record Query(Guid ToDoItemId) : IRequestWrapper<ToDoItem>;

        public class QueryHandler : IHandlerWrapper<Query, ToDoItem>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IApplicationDbContext _dbContext;
            private readonly IMediator _mediator;

            public QueryHandler(IApplicationDbContext dbContext, IMediator mediator, IHttpContextAccessor httpContextAccessor)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor)); ;
            }

            public async Task<Response<ToDoItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUserResponse = await _mediator.Send(new GetCurrentUser.GetCurrentUser.Query());
                if (currentUserResponse.Succeeded)
                {
                    var toDoItemFromDb = await _dbContext.ToDoItems
                        .AsNoTracking()
                        .SingleOrDefaultAsync(t => t.Id == request.ToDoItemId && t.UserId == currentUserResponse.Value.Id,
                        cancellationToken: cancellationToken);

                    if (toDoItemFromDb is null)
                        return Response<ToDoItem>.Fail();
                    else
                        return Response<ToDoItem>.Ok(toDoItemFromDb);
                }
                else
                {
                    return Response<ToDoItem>.Fail(currentUserResponse.Errors);
                }
            }
        }
    }
}