using Microsoft.AspNetCore.Identity;
using FluentValidation.Validators;
using Core.Domain.Entities;
using FluentValidation;

namespace Core.Application.Features.Commands.JwtRegister
{
    public partial class CreateUser
    {
        public class CommandValidator : AbstractValidator<CreateUser.Command>
        {
            public CommandValidator(UserManager<AppUser> userManager)
            {
                RuleFor(u => u.Email)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Email can't be empty")
                    .EmailAddress(EmailValidationMode.AspNetCoreCompatible).WithMessage("Invalid email")
                    //.MaximumLength(255).WithMessage("Maximum length is 255")
                    .MustAsync(async (email, _) => !string.IsNullOrEmpty(email) && await userManager.FindByEmailAsync(email) == null)
                        .WithMessage("Email already exists");

                RuleFor(u => u.Username)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Email can't be empty")
                    //.Matches(@"^(?=.{4,255}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$").WithMessage("Incorrect username")
                    .MustAsync(async (username, _) => !string.IsNullOrEmpty(username) && await userManager.FindByNameAsync(username) == null)
                        .WithMessage("Username already exists");

                RuleFor(u => u.PhoneNumber)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Phone number can't be empty")
                    .Matches(@"\d{11}").WithMessage("Enter valid phone number: \\d{11}");

                RuleFor(u => u.Password)
                    .Cascade(CascadeMode.Stop)
                    .NotEmpty().WithMessage("Password can't be empty");
                //.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,255}$")
                //.WithMessage("Min 8 / max 255 characters, at least one uppercase letter, one lowercase letter and one number");

                RuleFor(u => u.FirstName)
                    .NotEmpty().WithMessage("First name can't be empty")
                    .MaximumLength(64);

                RuleFor(u => u.LastName)
                    .NotEmpty().WithMessage("Last name can't be empty")
                    .MaximumLength(64);

                //RuleFor(u => u.ConfirmPassword)
                //    .Cascade(CascadeMode.Stop)
                //    .Equal(u => u.Password).WithMessage("Password mismatch");
            }
        }
    }
}