using Application.DTOs.Merchant;
using FluentValidation;

public class CreateMerchantDtoValidator : AbstractValidator<CreateMerchantDto>
{
    public CreateMerchantDtoValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100)
            .WithMessage("Merchant name must be between 2 and 100 characters.");
    }
}
