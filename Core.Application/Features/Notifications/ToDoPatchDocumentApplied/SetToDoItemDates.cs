using System.Threading.Tasks;
using System.Threading;
using MediatR;
using System;

namespace Core.Application.Features.Notifications.ToDoPatchDocumentApplied
{
    public class SetToDoItemDates : INotificationHandler<ToDoPatchDocumentApplied>
    {
        public Task Handle(ToDoPatchDocumentApplied notification, CancellationToken cancellationToken)
        {
            var toDoItemFromDb = notification.ToDoItem;

            var utcNow = DateTime.UtcNow;

            toDoItemFromDb.ModifiedAt = utcNow;
            if (toDoItemFromDb.Recurrence is not null)
                if (toDoItemFromDb.DueDate is null)
                    toDoItemFromDb.DueDate = utcNow.Date;

            return Task.CompletedTask;
        }
    }
}