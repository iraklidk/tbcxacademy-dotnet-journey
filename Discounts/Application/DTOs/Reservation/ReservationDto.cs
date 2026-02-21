namespace Application.DTOs.Reservation;

public class ReservationDto
{
    public int Id { get; set; }

    public int OfferId { get; set; }

    public int CustomerId { get; set; }

    public string OfferTitle { get; set; } = default!;

    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime ReservedAt { get; set; }

    public DateTime ExpiresAt { get; set; }
}
