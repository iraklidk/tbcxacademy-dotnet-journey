using Domain.Constants;

namespace Domain.Entities;

public class Coupon
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public int? CustomerId { get; set; }

    public Customer? Customer { get; set; } = null!;

    public DateTime ExpirationDate { get; set; }

    public DateTime? UsedAt { get; set; }

    public int? OfferId { get; set; }

    public Offer? Offer { get; set; } = null!;

    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    public CouponStatus Status { get; set; } = CouponStatus.Active;
}
