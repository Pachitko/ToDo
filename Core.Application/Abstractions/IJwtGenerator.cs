using System.Threading.Tasks;
using Core.Domain.Entities;

namespace Core.Application.Abstractions
{
    public interface IJwtGenerator
    {
        Task<string> CreateTokenAsync(AppUser user);
    }
}