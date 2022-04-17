using Core.Application.Features.Commands.CreateUser;

namespace UnitTests.Extensions.Core.Application
{
    public static class UserExtensions
    {
        private static class Defaults
        {
            public const string Username = "Username";
            public const string Email = "email@mail.ru";
            public const string Password = "Qwerty123";
            public const string PasswordConfirmation = "Qwerty123";
        }

        public static CreateUser.Command GetCreateUserCommand(
            string username = Defaults.Username, string email = Defaults.Email,
            string password = Defaults.Password, string passwordConfirmation = Defaults.PasswordConfirmation)
                => new(username, email, password, passwordConfirmation);

        public static CreateUser.Command WithUsername(this CreateUser.Command cmd, string username = Defaults.Username)
            => cmd with { Username = username };
        public static CreateUser.Command WithEmail(this CreateUser.Command cmd, string email = Defaults.Email)
            => cmd with { Email = email };
        public static CreateUser.Command WithPassword(this CreateUser.Command cmd, string password = Defaults.Password)
            => cmd with { Password = password };
        public static CreateUser.Command WithPasswordConfirmation(this CreateUser.Command cmd, string passwordConfirmation = Defaults.PasswordConfirmation)
            => cmd with { PasswordConfirmation = passwordConfirmation };
    }
}