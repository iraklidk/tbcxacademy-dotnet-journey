using Mapster;
using Domain.Entities;
using Application.Interfaces;
using Application.DTOs.Coupon;
using Application.Interfaces.Repos;
using Application.Interfaces.Services;
using Discounts.Application.Exceptions;

namespace Application.Services;

public class CouponService : ICouponService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOfferRepository _offerRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMerchantRepository _merchantRepository;

    public CouponService(IUnitOfWork unitOfWork,
                         IOfferRepository offerRepository,
                         ICouponRepository couponRepository,
                         IMerchantRepository merchantRepository,
                         ICustomerRepository customerRepository)
    {
        _unitOfWork = unitOfWork;
        _offerRepository = offerRepository;
        _couponRepository = couponRepository;
        _merchantRepository = merchantRepository;
        _customerRepository = customerRepository;
    }

    public async Task<List<CouponDto>> GetAllAsync(CancellationToken ct = default)
    {
        var coupons = await _couponRepository.GetAllAsync(ct).ConfigureAwait(false);
        return coupons.Adapt<List<CouponDto>>();
    }

    public async Task<CouponDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var coupon = await _couponRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (coupon == null) throw new NotFoundException($"Coupon with Id {id} not found!");
        return coupon.Adapt<CouponDto>();
    }

    public async Task<List<CouponDto>> GetByUserAsync(int userId, CancellationToken ct = default)
    {
        var coupons = await _couponRepository.GetByUserAsync(userId, ct).ConfigureAwait(false);
        return coupons.Adapt<List<CouponDto>>();
    }

    public async Task<List<CouponDto>> GetByOfferAsync(int offerId, CancellationToken ct = default)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId, ct).ConfigureAwait(false);
        if(offer == null) throw new NotFoundException($"Offer with Id {offerId} not found!");
        var coupons = await _couponRepository.GetByOfferAsync(offerId, ct).ConfigureAwait(false);
        return coupons.Adapt<List<CouponDto>>();
    }

    public async Task<List<CouponDto>> GetCouponsByMerchantAsync(int userId, CancellationToken ct = default)
    {
        var merchant = await _merchantRepository.GetMerchantByUserIdAsync(userId, ct).ConfigureAwait(false);
        if(merchant == null) throw new NotFoundException($"Merchant with User Id {userId} not found!");
        var merchantCoupons = await _couponRepository.GetCouponsByMerchantIdAsync(merchant.Id, ct).ConfigureAwait(false);
        return merchantCoupons.Adapt<List<CouponDto>>();
    }

    public async Task<CouponDto> CreateCouponAsync(CreateCouponDto dto, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct).ConfigureAwait(false);
        
        var offer = await _offerRepository.GetByIdAsync(dto.OfferId, ct).ConfigureAwait(false);
        if (offer == null) throw new NotFoundException($"Offer with id {dto.OfferId} not found!");
        var customer = await _customerRepository.GetCustomerByUserIdAsync(dto.UserId, ct).ConfigureAwait(false);
        if (customer == null) throw new NotFoundException($"User with user id {dto.UserId} not found!");

        if (customer.Balance < offer.DiscountedPrice) throw new DomainException("Customer does not have enough balance to buy the coupon!");
        customer.Balance -= offer.DiscountedPrice;
        var merchant = await _merchantRepository.GetByIdAsync(offer.MerchantId, ct).ConfigureAwait(false);
        merchant.Balance += offer.DiscountedPrice;

        var coupon = new Coupon
        {
            MerchantId = offer.MerchantId,
            CustomerName = $"{customer.Firstname} {customer.Lastname}",
            CustomerId = customer.Id,
            Code = $"C-{dto.UserId}-{offer.MerchantId}-{Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper()}",
            ExpirationDate = offer.EndDate,
            OfferId = dto.OfferId
        };

        await _couponRepository.AddAsync(coupon, ct).ConfigureAwait(false);
        offer.RemainingCoupons -= 1;
        await _offerRepository.UpdateAsync(offer, ct).ConfigureAwait(false);
        await _unitOfWork.CommitAsync(ct).ConfigureAwait(false);
        return coupon.Adapt<CouponDto>();
    }

    public async Task UpdateCouponAsync(UpdateCouponDto dto, CancellationToken ct = default)
    {
        var coupon = await _couponRepository.GetByIdAsync(dto.Id, ct).ConfigureAwait(false);
        if (coupon == null) throw new NotFoundException($"Coupon with Id {dto.Id} not found!");
        await _couponRepository.UpdateAsync(dto.Adapt(coupon), ct).ConfigureAwait(false);
    }

    public async Task DeleteCouponAsync(int id, CancellationToken ct = default)
    {
        var coupon = await _couponRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (coupon == null) throw new NotFoundException($"Coupon with Id {id} not found!");
        await _couponRepository.DeleteAsync(coupon, ct).ConfigureAwait(false);
    }

    public async Task<List<CouponDto?>> GetByCustomerAsync(int customerId, CancellationToken ct = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, ct).ConfigureAwait(false);
        if (customer == null) throw new NotFoundException($"Customer with id {customerId} not found!");
        var coupons = await _couponRepository.GetByCustomerAsync(customerId, ct).ConfigureAwait(false);
        if (coupons == null) throw new NotFoundException($"No coupons found for customer with Id {customerId}!");
        return coupons.Adapt<List<CouponDto?>>();
    }
}
