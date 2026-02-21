namespace Application.DTOs.Reservation;

public class UpdateReservationDto
{
    public int Id { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsActive { get; set; } 
}
