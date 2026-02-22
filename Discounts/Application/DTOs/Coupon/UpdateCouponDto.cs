using Domain.Constants;

namespace Application.DTOs.Coupon;

public class UpdateCouponDto
{
    public int Id { get; set; }

    public DateTime? UsedAt { get; set; }

    public CouponStatus Status { get; set; }

    public DateTime ExpirationDate { get; set; }
}
