using Domain.Entities;

namespace Application.Interfaces.Repos;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<List<Reservation>> GetByUserAsync(int userId, CancellationToken ct = default);

    Task<List<Reservation>> GetByOfferAsync(int offerId, CancellationToken ct = default);

    Task<bool> ExistsActiveAsync(int offerId, int customerId, CancellationToken ct = default);

    Task<IEnumerable<Reservation>> GetExpiredReservationsAsync(CancellationToken ct = default);

    Task<List<Reservation>> GetByCustomerAsync(int customerId, CancellationToken ct = default);
}
