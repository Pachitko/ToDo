using Core.Application.Abstractions;
using System.Threading.Tasks;
using System.Threading;
using MediatR;

namespace Core.Application.Features.Notifications.UserCreated
{
    public class SendEmailConfirmationCode : INotificationHandler<UserCreated>
    {
        private readonly IEmailConfirmationLinkSender _emailConfirmationLinkSender;
        public SendEmailConfirmationCode(IEmailConfirmationLinkSender emailConfirmationLinkSender)
        {
            _emailConfirmationLinkSender = emailConfirmationLinkSender ?? throw new System.ArgumentNullException(nameof(emailConfirmationLinkSender));
        }
        public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
        {
            await _emailConfirmationLinkSender.SendConfirmationCodeAsync(notification.CreatedUser);
        }
    }
}