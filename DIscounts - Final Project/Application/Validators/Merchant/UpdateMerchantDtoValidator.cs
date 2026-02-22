using FluentValidation;
using Application.DTOs.Merchant;

public class UpdateMerchantDtoValidator : AbstractValidator<UpdateMerchantDto>
{
    public UpdateMerchantDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Merchant ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100)
            .WithMessage("Merchant name must be between 2 and 100 characters.");
    }
}
