namespace Application.DTOs.Reservation;

public class ReservationDto
{
    public int Id { get; set; }

    public int OfferId { get; set; }

    public decimal Price { get; set; }

    public int CustomerId { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime ReservedAt { get; set; }

    public bool IsActive { get; set; } = true;

    public string OfferTitle { get; set; } = default!;
}
