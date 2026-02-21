using Application.DTOs.Offer;
using FluentValidation;

public class UpdateOfferStatusDtoValidator : AbstractValidator<UpdateOfferStatusDto>
{
    public UpdateOfferStatusDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Offer ID is required.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid status value.");
    }
}
