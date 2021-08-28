using Core.Domain.Entities;

namespace Core.Application.Features.Commands.JwtRegister
{
    public partial class CreateUser
    {
        public record Command : IRequestWrapper<AppUser>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
            //public string ConfirmPassword { get; set; }
        }
    }
}