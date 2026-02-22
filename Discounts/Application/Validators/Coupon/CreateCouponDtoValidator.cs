using FluentValidation;
using Application.DTOs.Coupon;

public class CreateCouponDtoValidator : AbstractValidator<CreateCouponDto>
{
    public CreateCouponDtoValidator()
    {
        RuleFor(x => x.OfferId)
            .GreaterThan(0)
            .WithMessage("Offer ID is required.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(x => x.PurchasedAt);
    }
}
