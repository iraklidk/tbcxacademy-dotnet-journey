using Domain.Constants;

namespace Application.DTOs.Offer;

public class UpdateOfferDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public decimal DiscountedPrice { get; set; }

    public int RemainingCoupons { get; set; }

    public DateTime EndDate { get; set; }

    public OfferStatus Status { get; set; } = OfferStatus.Pending;

    public DateTime Updated { get; set; } = DateTime.UtcNow;
}
