namespace Application.DTOs.Reservation;

public class CreateReservationDto
{
    public int OfferId { get; set; }

    public int UserId { get; set; } 

    public DateTime ExpiresAt { get; set; }
}
