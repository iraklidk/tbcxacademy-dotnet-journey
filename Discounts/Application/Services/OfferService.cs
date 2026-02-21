using Discounts.Application.Exceptions;
using Application.Interfaces.Services;
using Application.Interfaces.Repos;
using Application.DTOs.Offer;
using Application.Interfaces;
using Domain.Entities;
using Mapster;

namespace Application.Services;

public class OfferService : IOfferService
{
    private readonly IGlobalSettingsRepository _globalSettingsRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMerchantRepository _merchantRepository;
    private readonly IOfferRepository _offerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OfferService(IGlobalSettingsRepository globalSettingsRepository,
                        IReservationRepository reservationRepository,
                        ICustomerRepository customerRepository,
                        ICategoryRepository categoryRepository,
                        IMerchantRepository merchantRepository,
                        IOfferRepository offerRepository,
                        IUnitOfWork unitOfWork)
    {
        _globalSettingsRepository = globalSettingsRepository;
        _reservationRepository = reservationRepository;
        _categoryRepository = categoryRepository;
        _merchantRepository = merchantRepository;
        _customerRepository = customerRepository;
        _offerRepository = offerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OfferDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var offer = await _offerRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (offer == null) throw new NotFoundException($"Offer with Id {id} not found!");
        return offer.Adapt<OfferDto>();
    }

    public async Task<OfferDto> CreateOfferAsync(CreateOfferDto dto, CancellationToken ct)
    {
        if (dto.DiscountedPrice >= dto.OriginalPrice) throw new DomainException("Discounted price must be lower than original price!");
        if (dto.EndDate <= dto.StartDate) throw new DomainException("End date must be after start date!");
        var merchant = await _merchantRepository.GetMerchantByUserIdAsync(dto.UserId, ct).ConfigureAwait(false);
        if (merchant == null) throw new NotFoundException("Merchant doesn't exists!");

        var offer = dto.Adapt<Offer>();
        offer.MerchantId = merchant.Id;
        offer.RemainingCoupons = dto.TotalCoupons;
        await _offerRepository.AddAsync(offer, ct).ConfigureAwait(false);
        return offer.Adapt<OfferDto>();
    }

    public async Task UpdateOfferAsync(UpdateOfferDto dto, CancellationToken ct)
    {
        var offer = await _offerRepository.GetByIdAsync(dto.Id, ct).ConfigureAwait(false);
        if (offer == null) throw new NotFoundException($"Offer with Id {dto.Id} not found.!");
        if (dto.DiscountedPrice > offer.OriginalPrice) throw new DomainException("Discounted price must be lower than original price!");
        if (dto.EndDate <= offer.StartDate) throw new DomainException("End date must be after start date!");
        if (dto.RemainingCoupons > offer.RemainingCoupons) throw new DomainException("Remaining coupons cannot exceed latest remaining coupons!");

        var settings = await _globalSettingsRepository.GetByIdAsync(1, ct).ConfigureAwait(false);
        if ((DateTime.UtcNow - offer.Created).TotalHours > settings.MerchantEditHours)
            throw new DomainException($"Offers cannot be edited after a {settings.MerchantEditHours} hours from the start date!");
        dto.Adapt(offer);
        offer.Updated = DateTime.UtcNow;
        await _offerRepository.UpdateAsync(offer, ct).ConfigureAwait(false);
    }

    public async Task DeleteOfferAsync(int id, CancellationToken ct)
    {
        var offer = await _offerRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (offer == null) throw new NotFoundException($"Offer with Id {id} not found!");
        await _offerRepository.DeleteAsync(offer, ct).ConfigureAwait(false);
    }

    public async Task<IEnumerable<OfferDto>> GetAllAsync(CancellationToken ct = default)
    {
        var offers = await _offerRepository.GetAllAsync(ct).ConfigureAwait(false);
        var offerDtos = offers.Adapt<List<OfferDto>>();

        var categoryIds = offers.Select(x => x.CategoryId).Distinct().ToList();
        var categories = await _categoryRepository.GetByIdsAsync(categoryIds, ct).ConfigureAwait(false);
        var categoryDict = categories.ToDictionary(x => x.Id, x => x.Name);
        foreach (var dto in offerDtos)
            if (categoryDict.TryGetValue(dto.CategoryId, out var categoryName)) dto.Category = categoryName;

        return offerDtos;
    }

    public async Task<IEnumerable<OfferDto>> GetPendingsAsync(CancellationToken ct = default)
    {
        var entities = await _offerRepository.GetPendingsAsync(ct).ConfigureAwait(false);
        return entities.Adapt<List<OfferDto>>();
    }

    public async Task UpdateStatusAsync(UpdateOfferStatusDto dto, CancellationToken ct)
    {
        var offer = await _offerRepository.GetByIdAsync(dto.Id, ct).ConfigureAwait(false);
        if (offer == null) throw new NotFoundException($"Offer with Id {dto.Id} not found!");
        await _offerRepository.UpdateAsync(dto.Adapt(offer), ct).ConfigureAwait(false);
    }

    public async Task<bool> IsOfferReservedByUserAsync(int offerId, int userId, CancellationToken ct = default)
    {
        var customer = await _customerRepository.GetCustomerByUserIdAsync(userId, ct).ConfigureAwait(false);
        if (customer == null) throw new NotFoundException("Customer doesn't exists!");
        return await _reservationRepository.ExistsActiveAsync(offerId, customer.Id, ct).ConfigureAwait(false);
    }

    public async Task ChangeRemainingCouponsAsync(int offerId, int count = 1, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct).ConfigureAwait(false);

        var offer = await _offerRepository.GetByIdAsync(offerId, ct).ConfigureAwait(false);
        if (offer == null) throw new NotFoundException($"Offer with id {offerId} not found!");
        await _offerRepository.ChangeRemainingCouponsAsync(offerId, count, ct).ConfigureAwait(false);

        await _unitOfWork.CommitAsync(ct).ConfigureAwait(false);
    }

    public async Task<IEnumerable<OfferDto>> GetByMerchantIdAsync(int merchantId, CancellationToken ct = default)
        => (await _offerRepository.GetByMerchantIdAsync(merchantId, ct).ConfigureAwait(false)).Adapt<IEnumerable<OfferDto>>();
}
