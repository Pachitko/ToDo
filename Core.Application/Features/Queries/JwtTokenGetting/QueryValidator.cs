using FluentValidation;

namespace Core.Application.Features.Queries.JwtLogin
{
    public partial class JwtTokenGetting
    {
        public class QueryValidator : AbstractValidator<JwtTokenGetting.Query>
        {
            public QueryValidator()
            {
                RuleFor(u => u.Username)
                    .NotEmpty().WithMessage("Email is empty");
                //.EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email");

                RuleFor(u => u.Password)
                    .NotEmpty().WithMessage("Password is empty");
            }
        }
    }
}