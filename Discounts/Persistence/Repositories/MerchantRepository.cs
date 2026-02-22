using Domain.Entities;
using Application.Interfaces.Repos;
using Discounts.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Discounts.Persistence.Repositories;

public class MerchantRepository : BaseRepository<Merchant>, IMerchantRepository
{
    public MerchantRepository(DiscountsDbContext context) : base(context) { }

    public Task<Merchant?> GetMerchantByUserIdAsync(int userId, CancellationToken ct = default)
        => _context.Merchants.AsNoTracking().Where(m => m.UserId == userId).FirstOrDefaultAsync(ct);
}
