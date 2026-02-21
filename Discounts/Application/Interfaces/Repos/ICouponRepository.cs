using Domain.Entities;

namespace Application.Interfaces.Repos;

public interface ICouponRepository : IBaseRepository<Coupon>
{
    Task<List<Coupon>> GetCouponsByMerchantIdAsync(int merchantId, CancellationToken ct = default);

    Task<List<Coupon>> GetByUserAsync(int customerId, CancellationToken ct = default);

    Task<List<Coupon>> GetByOfferAsync(int offerId, CancellationToken ct = default);

    Task<Coupon> GetByCustomerAsync(int customerId, CancellationToken ct = default);
}
