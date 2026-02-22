using FluentValidation;
using Application.DTOs.Coupon;

public class UpdateCouponDtoValidator : AbstractValidator<UpdateCouponDto>
{
    public UpdateCouponDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Coupon ID is required.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid coupon status.");

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.MinValue)
            .WithMessage("Expiration date must be valid.");

        RuleFor(x => x.UsedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.UsedAt.HasValue)
            .WithMessage("Used date cannot be in the future.");
    }
}
