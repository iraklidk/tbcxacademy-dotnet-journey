namespace Domain.Entities;

public class Reservation
{
    public int Id { get; set; }

    public int OfferId { get; set; }

    public Offer Offer { get; set; } = null!;

    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = null!;

    public DateTime ReservedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpiresAt { get; set; }

    public bool IsActive { get; set; } = true;
}
