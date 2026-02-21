using Domain.Constants;

namespace Application.DTOs.Coupon;

public class CouponDto
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public int CustomerId { get; set; }

    public string? Code { get; set; }

    public CouponStatus Status { get; set; }

    public DateTime PurchasedAt { get; set; }

    public DateTime ExpirationDate { get; set; }

    public DateTime UsedAt { get; set; }
}
