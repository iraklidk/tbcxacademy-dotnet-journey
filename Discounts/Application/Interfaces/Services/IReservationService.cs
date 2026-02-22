using Application.DTOs.Reservation;

namespace Application.Interfaces.Services;

public interface IReservationService
{
    Task DeleteReservationAsync(int id, CancellationToken ct = default);

    Task<ReservationDto> GetByIdAsync(int id, CancellationToken ct = default);

    Task<IEnumerable<ReservationDto>> GetAllAsync(CancellationToken ct = default);

    Task UpdateReservationAsync(UpdateReservationDto dto, CancellationToken ct = default);

    Task<List<ReservationDto>> GetByUserAsync(int userId, CancellationToken ct = default);

    Task<bool> ExistsActiveAsync(int offerId, int customerId, CancellationToken ct = default);

    Task<IEnumerable<ReservationDto>> GetByOfferAsync(int offerId, CancellationToken ct = default);

    Task<IEnumerable<ReservationDto>> GetByCustomerAsync(int customerId, CancellationToken ct = default);

    Task<ReservationDto> CreateReservationAsync(CreateReservationDto dto, CancellationToken ct = default);
}
