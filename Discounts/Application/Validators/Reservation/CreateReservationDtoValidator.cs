using Application.DTOs.Reservation;
using FluentValidation;

public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(x => x.OfferId)
            .GreaterThan(0)
            .WithMessage("Offer is required.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User is required.");

        RuleFor(x => x.ExpiresAt)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("Expiration date must be in the future.");
    }
}
