using Application.DTOs.Reservation;
using Swashbuckle.AspNetCore.Filters;

namespace API.Infrastructure.SwaggerExamples;

public class ReservationExamples
{
    public class ReservationDtoExample : IExamplesProvider<ReservationDto>
    {
        public ReservationDto GetExamples()
        {
            return new ReservationDto
            {
                Id = 123,
                OfferId = 456,
                CustomerId = 789,
                ReservedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }
    }

    public class CreateReservationDtoExample : IExamplesProvider<CreateReservationDto>
    {
        public CreateReservationDto GetExamples()
        {
            return new CreateReservationDto
            {
                UserId = 5,
                OfferId = 456,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }
    }

    public class UpdateReservationDtoExample : IExamplesProvider<UpdateReservationDto>
    {
        public UpdateReservationDto GetExamples()
        {
            return new UpdateReservationDto
            {
                Id = 123,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }
    }
}
