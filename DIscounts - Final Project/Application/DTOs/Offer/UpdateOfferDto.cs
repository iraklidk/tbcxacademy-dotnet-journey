using Domain.Constants;

namespace Application.DTOs.Offer;

public class UpdateOfferDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public DateTime EndDate { get; set; }

    public string? Description { get; set; }

    public int RemainingCoupons { get; set; }

    public decimal DiscountedPrice { get; set; }

    public DateTime Updated { get; set; } = DateTime.UtcNow;

    public OfferStatus Status { get; set; } = OfferStatus.Pending;
}
