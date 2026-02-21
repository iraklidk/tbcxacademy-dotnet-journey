using Domain.Constants;

namespace Domain.Entities;

public class Offer
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal OriginalPrice { get; set; }

    public decimal DiscountedPrice { get; set; }

    public int TotalCoupons { get; set; }

    public int RemainingCoupons { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime Updated { get; set; } = DateTime.UtcNow;

    public OfferStatus Status { get; set; } = OfferStatus.Pending;

    public Merchant Merchant { get; set; } = null!;

    public int MerchantId { get; set; }

    public Category Category { get; set; } = null!;

    public int CategoryId { get; set; }

    public ICollection<Coupon>? Coupons { get; set; } = new List<Coupon>();

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
