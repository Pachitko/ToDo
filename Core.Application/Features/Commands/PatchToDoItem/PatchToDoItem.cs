﻿using Core.Application.Features.Queries.GetToDoItemById;
using Microsoft.AspNetCore.JsonPatch;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Infrastructure.Data;
using System.Threading;
using System.Linq;
using AutoMapper;
using MediatR;
using System;

namespace Core.Application.Features.Commands.PatchToDoItem
{
    public partial class PatchToDoItem
    {
        public record Command(Guid ToDoItemId, JsonPatchDocument<ToDoItem> jsonPatchDocument) : IRequestWrapper<ToDoItem>;

        public class CommandHandler : IHandlerWrapper<PatchToDoItem.Command, ToDoItem>
        {
            private readonly IApplicationDbContext _dbContext;
            private readonly IMediator _mediator;

            public CommandHandler(IApplicationDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext;
                _mediator = mediator;
            }

            public async Task<Response<ToDoItem>> Handle(PatchToDoItem.Command request, CancellationToken cancellationToken)
            {
                var response = await _mediator.Send(new GetToDoItemById.Query(request.ToDoItemId), cancellationToken);
                if (response.Succeeded)
                {
                    var toDoItemFromDb = _dbContext.ToDoItems.Attach(response.Value).Entity;
                    request.jsonPatchDocument.ApplyTo(toDoItemFromDb);

                    toDoItemFromDb.ModifiedAt = DateTime.UtcNow;
                    if(toDoItemFromDb.Recurrence is not null)
                        if (toDoItemFromDb.DueDate is null)
                            toDoItemFromDb.DueDate = DateTime.Today;

                    var updatedEntry = _dbContext.ToDoItems.Update(toDoItemFromDb);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    return Response<ToDoItem>.Ok(updatedEntry.Entity);
                }
                else
                {
                    return Response<ToDoItem>.Fail(response.Errors);
                }
            }
        }
    }
}