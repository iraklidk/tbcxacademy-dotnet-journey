using Application.DTOs.Reservation;
using Application.Interfaces.Repos;
using Discounts.Persistence.Context;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Discounts.Persistence.Repositories;

public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(DiscountsDbContext context) : base(context) { }

    public Task<List<Reservation>> GetByCustomerAsync(int customerId, CancellationToken ct = default)
        => _context.Reservations.Where(r => r.CustomerId == customerId).Include(r => r.Customer).ToListAsync(ct);

    public Task<List<Reservation>> GetByOfferAsync(int offerId, CancellationToken ct = default)
        => _context.Reservations.Where(r => r.OfferId == offerId).Include(r => r.Offer).ToListAsync(ct);

    public Task<bool> ExistsActiveAsync(int offerId, int customerId, CancellationToken ct = default)
        => _context.Reservations.AnyAsync(r => r.OfferId == offerId && r.CustomerId == customerId && r.IsActive, ct);

    public async Task<IEnumerable<Reservation>> GetExpiredReservationsAsync(CancellationToken ct = default)
        => await _context.Reservations.Where(r => r.IsActive && r.ExpiresAt <= DateTime.UtcNow).ToListAsync(ct).ConfigureAwait(false);

    public async Task<List<ReservationDto>> GetByUserAsync(int userId, CancellationToken ct = default)
    {
        return await _context.Reservations.Include(r => r.Customer).Include(r => r.Offer).Where(r => r.Customer.UserId == userId)
                    .Select(r => new ReservationDto
                    {
                        Id = r.Id,
                        OfferId = r.OfferId,
                        OfferTitle = r.Offer.Title,
                        CustomerId = r.CustomerId,
                        Price = r.Offer.DiscountedPrice,
                        ReservedAt = r.ReservedAt,
                        ExpiresAt = r.ExpiresAt
                    })
                    .ToListAsync(ct).ConfigureAwait(false);
    }
}
