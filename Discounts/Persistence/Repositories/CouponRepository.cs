using Domain.Entities;
using Application.Interfaces.Repos;
using Discounts.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Discounts.Persistence.Repositories;

public class CouponRepository : BaseRepository<Coupon>, ICouponRepository
{
    public CouponRepository(DiscountsDbContext context) : base(context) { }

    public Task<List<Coupon>> GetByUserAsync(int userId, CancellationToken ct = default)
        => _context.Coupons.Include(c => c.Customer).Where(c => c.Customer.UserId == userId).ToListAsync(ct);

    public Task<List<Coupon>> GetByOfferAsync(int offerId, CancellationToken ct = default)
        => _context.Coupons.Where(c => c.OfferId == offerId).Include(c => c.Offer).ToListAsync(ct);

    public Task<List<Coupon>> GetCouponsByMerchantIdAsync(int merchantId, CancellationToken ct = default)
        => _context.Coupons.Include(c => c.Offer).Where(c => c.Offer.MerchantId == merchantId).ToListAsync(ct);

    public Task<List<Coupon?>> GetByCustomerAsync(int customerId, CancellationToken ct = default)
        => _context.Coupons.Include(c => c.Customer).Where(c => c.CustomerId == customerId).ToListAsync(ct);
}
