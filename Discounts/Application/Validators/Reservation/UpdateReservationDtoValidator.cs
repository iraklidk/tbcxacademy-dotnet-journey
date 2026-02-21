using Application.DTOs.Reservation;
using FluentValidation;

public class UpdateReservationDtoValidator : AbstractValidator<UpdateReservationDto>
{
    public UpdateReservationDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Reservation ID is required.");

        RuleFor(x => x.ExpiresAt)
            .Must(date => date > DateTime.UtcNow)
            .WithMessage("Expiration date must be in the future.");
    }
}
