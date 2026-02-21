using Application.DTOs.Coupon;

namespace Application.Interfaces.Services;

public interface ICouponService
{
    Task<List<CouponDto>> GetCouponsByMerchantAsync(int merchantId, CancellationToken ct = default);

    Task<CouponDto> CreateCouponAsync(CreateCouponDto dto, CancellationToken ct = default);

    Task<List<CouponDto>> GetByUserAsync(int customerId, CancellationToken ct = default);

    Task<CouponDto?> GetByCustomerAsync(int customerId, CancellationToken ct = default);

    Task<List<CouponDto>> GetByOfferAsync(int offerId, CancellationToken ct = default);

    Task UpdateCouponAsync(UpdateCouponDto dto, CancellationToken ct = default);

    Task<CouponDto> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<CouponDto>> GetAllAsync(CancellationToken ct = default);

    Task DeleteCouponAsync(int id, CancellationToken ct = default);
}
