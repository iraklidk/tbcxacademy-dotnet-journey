namespace Application.DTOs.Coupon;

public class CreateCouponDto
{
    public int UserId { get; set; }

    public int OfferId { get; set; }

    public string Code { get; set; }

    public DateTime ExpirationDate { get; set; }

    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
}
