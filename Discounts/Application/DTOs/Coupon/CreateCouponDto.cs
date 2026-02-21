namespace Application.DTOs.Coupon;

public class CreateCouponDto
{
    public int OfferId { get; set; }

    public int UserId { get; set; }

    public string Code { get; set; }

    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpirationDate { get; set; }
}
