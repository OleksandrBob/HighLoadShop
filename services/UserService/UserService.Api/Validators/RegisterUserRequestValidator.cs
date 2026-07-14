using FluentValidation;
using UserService.Api.Models;

namespace UserService.Api.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.FirstName)
            .MaximumLength(100)
            .WithMessage("FirstName cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .MaximumLength(100)
            .WithMessage("LastName cannot exceed 100 characters.");
    }
}