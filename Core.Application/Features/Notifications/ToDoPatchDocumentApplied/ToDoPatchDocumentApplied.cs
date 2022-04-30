using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Notifications.ToDoPatchDocumentApplied
{
    public record ToDoPatchDocumentApplied(ToDoItem ToDoItem) : INotification;
}