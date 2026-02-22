namespace Application.DTOs.Reservation;

public class CreateReservationDto
{
    public int UserId { get; set; } 

    public int OfferId { get; set; }

    public DateTime ExpiresAt { get; set; } = DateTime.Now.AddMinutes(10);
}
