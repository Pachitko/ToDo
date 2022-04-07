using System.Threading.Tasks;

namespace Core.Application.Abstractions.ExternalLogin
{
    public interface IExternalLoginProvider
    {
        Task<ExternalLoginPayload> GetPayloadAsync(string tokenId);
    }
}