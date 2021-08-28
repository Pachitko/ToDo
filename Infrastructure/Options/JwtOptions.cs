using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure.Options
{
    public class JwtOptions
    {
        public string Key { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}