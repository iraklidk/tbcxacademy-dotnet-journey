using Discounts.Application.Exceptions;
using Application.Interfaces.Services;
using Application.Interfaces.Repos;
using Application.DTOs.Reservation;
using Application.Interfaces;
using Domain.Entities;
using Mapster;

namespace Application.Services;

public class ReservationService : IReservationService
{
    private readonly IGlobalSettingsRepository _globalSettingsRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IOfferRepository _offerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReservationService(IGlobalSettingsRepository globalSettingsRepository,
                              IReservationRepository reservationRepository,
                              ICustomerRepository customerRepository,
                              IOfferRepository offerRepository,
                              IUnitOfWork unitOfWork)
    {
        _globalSettingsRepository = globalSettingsRepository;
        _reservationRepository = reservationRepository;
        _customerRepository = customerRepository;
        _offerRepository = offerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ReservationDto>> GetAllAsync(CancellationToken ct)
        => (await _reservationRepository.GetAllAsync(ct).ConfigureAwait(false)).Adapt<IEnumerable<ReservationDto>>();

    public async Task<ReservationDto> GetByIdAsync(int id, CancellationToken ct)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (reservation == null) throw new NotFoundException($"Reservation with Id {id} not found!");
        return reservation.Adapt<ReservationDto>();
    }

    public async Task<IEnumerable<ReservationDto>> GetByCustomerAsync(int customerId, CancellationToken ct)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, ct).ConfigureAwait(false);
        if(customer == null) throw new NotFoundException($"Customer with id {customerId} not found!");
        var reservations = await _reservationRepository.GetByCustomerAsync(customerId, ct).ConfigureAwait(false);
        return reservations.Adapt<List<ReservationDto>>();
    }

    public async Task<IEnumerable<ReservationDto>> GetByOfferAsync(int offerId, CancellationToken ct)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId, ct).ConfigureAwait(false);
        if (offer == null) throw new NotFoundException($"Offer with id {offerId} not found!");
        var reservations = await _reservationRepository.GetByOfferAsync(offerId, ct).ConfigureAwait(false);
        return reservations.Adapt<List<ReservationDto>>();
    }

    public async Task<ReservationDto> CreateReservationAsync(CreateReservationDto dto, CancellationToken ct)
    {
        await _unitOfWork.BeginTransactionAsync(ct).ConfigureAwait(false);

        var customer = await _customerRepository.GetCustomerByUserIdAsync(dto.UserId, ct).ConfigureAwait(false);
        if(customer == null) throw new NotFoundException($"User with user id {dto.UserId} not found!");

        var offer = await _offerRepository.GetByIdAsync(dto.OfferId, ct).ConfigureAwait(false);
        if (offer == null) throw new NotFoundException($"Offer with id {dto.OfferId} not found!");

        var settings = await _globalSettingsRepository.GetByIdAsync(1, ct).ConfigureAwait(false);
        if (customer.Balance < settings.ReservationPrice) throw new DomainException("Customer does not have enough balance to make a reservation!");
        customer.Balance -= settings.ReservationPrice;

        if (offer.RemainingCoupons <= 0) throw new DomainException("No coupons available for this offer!");
        await _offerRepository.ChangeRemainingCouponsAsync(dto.OfferId, -1, ct).ConfigureAwait(false);

        if(dto.ExpiresAt <= DateTime.UtcNow) throw new DomainException("Expiration date must be in the future!");
        
        var reservation = new Reservation
        {
            OfferId = dto.OfferId,
            CustomerId = customer.Id,
            ReservedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(settings.BookingDurationMinutes),
            IsActive = true
        };
        
        await _reservationRepository.AddAsync(reservation, ct).ConfigureAwait(false);
        await _unitOfWork.CommitAsync(ct).ConfigureAwait(false);
        return reservation.Adapt<ReservationDto>();
    }

    public async Task UpdateReservationAsync(UpdateReservationDto dto, CancellationToken ct)
    {
        var reservation = await _reservationRepository.GetByIdAsync(dto.Id, ct).ConfigureAwait(false);
        if (reservation == null) throw new NotFoundException($"Reservation with Id {dto.Id} not found!");
        if (reservation.ReservedAt >= dto.ExpiresAt) throw new DomainException("Reservation expiration must be after reserved date!");
        await _reservationRepository.UpdateAsync(dto.Adapt(reservation), ct).ConfigureAwait(false);
    }

    public async Task DeleteReservationAsync(int id, CancellationToken ct)
    {
        var reservation = await _reservationRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (reservation == null) throw new NotFoundException($"Reservation with Id {id} not found!");
        await _reservationRepository.DeleteAsync(reservation, ct).ConfigureAwait(false);
    }

    public Task<bool> ExistsActiveAsync(int offerId, int customerId, CancellationToken ct = default)
        => _reservationRepository.ExistsActiveAsync(offerId, customerId, ct);

    public async Task<List<ReservationDto>> GetByUserAsync(int userId, CancellationToken ct = default)
    {
        var reservations = await _reservationRepository.GetByUserAsync(userId, ct).ConfigureAwait(false);
        return reservations.Adapt<List<ReservationDto>>();
    }
}
