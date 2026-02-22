using Domain.Entities;
using Domain.Constants;
using Application.Interfaces.Repos;
using Microsoft.EntityFrameworkCore;
using Discounts.Persistence.Context;

namespace Discounts.Persistence.Repositories;

public class OfferRepository : BaseRepository<Offer>, IOfferRepository
{
    public OfferRepository(DiscountsDbContext context) : base(context) { }

    public Task<List<Offer>> GetByMerchantIdAsync(int merchantId, CancellationToken ct = default)
        => _context.Offers.Where(o => o.MerchantId == merchantId).ToListAsync(ct);

    public Task<List<Offer>> GetPendingsAsync(CancellationToken ct = default)
        => _context.Offers.Where(o => o.Status == OfferStatus.Pending).ToListAsync(ct);

    public Task ChangeRemainingCouponsAsync(int offerId, int count = 1, CancellationToken ct = default)
    {
        var offer = _context.Offers.FirstOrDefault(o => o.Id == offerId);
        offer.RemainingCoupons += count;
        return _context.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<Offer>> GetExpiredOffersAsync(CancellationToken ct = default)
        => await _context.Offers.Where(o => o.EndDate <= DateTime.UtcNow).ToListAsync(ct).ConfigureAwait(false);

    public Task<List<Offer>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
        => _context.Offers.Where(x => ids.Contains(x.Id)).ToListAsync(ct);
}
