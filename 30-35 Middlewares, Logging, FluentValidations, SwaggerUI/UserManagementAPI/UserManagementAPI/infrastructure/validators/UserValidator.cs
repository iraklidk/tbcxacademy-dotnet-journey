using FluentValidation;
using Microsoft.Extensions.Localization;
using UserManagement.Domain.Entity;

namespace UserManagementAPI.infrastructure.validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator(IStringLocalizer<UserValidator> localizer)
        {
            RuleFor(u => u.FirstName).NotEmpty().WithMessage(localizer["Firstname"]);
            RuleFor(u => u.LastName).NotEmpty().WithMessage(localizer["Lastname"]);
            RuleFor(u => u.Email).NotEmpty().WithMessage(localizer["EmailRequired"]).EmailAddress().WithMessage(localizer["EmailInvalid"]);
            RuleFor(u => u.Age).InclusiveBetween(18, 120).WithMessage(localizer["AgeInvalid"]);
        }
    }
}
