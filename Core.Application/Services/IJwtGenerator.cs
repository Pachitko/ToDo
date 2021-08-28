using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Application.Services
{
    public interface IJwtGenerator
    {
        Task<string> CreateTokenAsync(AppUser user);
    }
}