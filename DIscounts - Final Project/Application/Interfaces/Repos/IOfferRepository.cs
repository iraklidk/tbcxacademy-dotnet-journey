using Domain.Entities;

namespace Application.Interfaces.Repos;

public interface IOfferRepository : IBaseRepository<Offer>
{
    Task<List<Offer>> GetPendingsAsync(CancellationToken ct = default);

    Task<List<Offer>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct);

    Task<IEnumerable<Offer>> GetExpiredOffersAsync(CancellationToken ct = default);

    Task<List<Offer>> GetByMerchantIdAsync(int merchantId, CancellationToken ct = default);

    Task ChangeRemainingCouponsAsync(int offerId, int count = 1, CancellationToken ct = default);
}
