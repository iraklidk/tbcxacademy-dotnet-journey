using Domain.Entities;

namespace Application.Interfaces.Repos;

public interface IMerchantRepository : IBaseRepository<Merchant>
{
    Task<Merchant> GetMerchantByUserIdAsync(int userId, CancellationToken ct = default);
}
