using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Notifications.UserCreated
{
    public record UserCreated(AppUser CreatedUser) : INotification;
}