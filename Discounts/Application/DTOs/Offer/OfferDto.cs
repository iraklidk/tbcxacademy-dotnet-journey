using Domain.Constants;

namespace Application.DTOs.Offer;

public class OfferDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public ushort CouponPrice { get; set; }

    public decimal OriginalPrice { get; set; }

    public decimal DiscountedPrice { get; set; }

    public int TotalCoupons { get; set; }

    public int RemainingCoupons { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public OfferStatus Status { get; set; }

    public int MerchantId { get; set; }

    public string? MerchantName { get; set; }

    public int CategoryId { get; set; }

    public string? Category { get; set; }

    public int ReservationsCount { get; set; }

    public int CouponsCount { get; set; }
}
