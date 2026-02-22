using FluentValidation;
using Application.DTOs.User;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.RoleId)
            .GreaterThan(0);

        RuleFor(x => x.UserName)
            .Matches(@"^\S+$").WithMessage("Username cannot contain spaces");
    }
}
