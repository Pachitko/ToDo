using Core.Domain.Entities;
using System.Threading.Tasks;

namespace Core.Application.Abstractions
{
    public interface IEmailConfirmationLinkSender
    {
        Task SendConfirmationCodeAsync(AppUser user);
    }
}