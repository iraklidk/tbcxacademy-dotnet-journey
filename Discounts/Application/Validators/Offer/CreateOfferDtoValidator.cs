using FluentValidation;
using Application.DTOs.Offer;

public class CreateOfferDtoValidator : AbstractValidator<CreateOfferDto>
{
    public CreateOfferDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(150);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.OriginalPrice)
            .GreaterThan(0);

        RuleFor(x => x.DiscountedPrice)
            .GreaterThan(0)
            .LessThan(x => x.OriginalPrice)
            .WithMessage("Discounted price must be less than original price.");

        RuleFor(x => x.TotalCoupons)
            .GreaterThan(0);

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.UserId)
            .GreaterThan(0);
    }
}
