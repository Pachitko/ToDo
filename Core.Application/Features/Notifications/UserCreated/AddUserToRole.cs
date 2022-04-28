using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using MediatR;

namespace Core.Application.Features.Notifications.UserCreated
{
    public class AddUserToRole : INotificationHandler<UserCreated>
    {
        private readonly UserManager<AppUser> _userManager;

        public AddUserToRole(UserManager<AppUser> userManager)
        {
            _userManager = userManager ?? throw new System.ArgumentNullException(nameof(userManager));
        }
        public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
        {
            await _userManager.AddToRoleAsync(notification.CreatedUser, "User");
        }
    }
}