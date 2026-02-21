using Application.DTOs.Auth;
using FluentValidation;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email is required.");

        RuleFor(x => x.Firstname)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Firstname is required and cannot exceed 50 characters.");

        RuleFor(x => x.Lastname)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Lastname is required and cannot exceed 50 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?\d{7,15}$")
            .WithMessage("Phone number is required and must be valid.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .WithMessage("Username must be between 3 and 50 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Role is required.");
    }
}
