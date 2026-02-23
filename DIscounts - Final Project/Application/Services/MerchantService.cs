using Mapster;
using Domain.Entities;
using Application.DTOs.Offer;
using Application.DTOs.Merchant;
using Application.Interfaces.Repos;
using Application.Interfaces.Services;
using Discounts.Application.Exceptions;

namespace Application.Services;

public class MerchantService : IMerchantService
{
    private readonly IUserService _userService;
    private readonly IOfferRepository _offerRepository;
    private readonly IMerchantRepository _merchantRepository;
    private readonly ICategoryRepository _categoryRepository;

    public MerchantService(IUserService userService,
                           IOfferRepository offerRepository,
                           IMerchantRepository merchantRepository,
                           ICategoryRepository categoryRepository)
    {
        _userService = userService;
        _offerRepository = offerRepository;
        _merchantRepository = merchantRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<MerchantResponseDto> GetMerchantByUserIdAsync(int userId, CancellationToken ct = default)
    {
        var merchant = await _merchantRepository.GetMerchantByUserIdAsync(userId, ct).ConfigureAwait(false);
        if (merchant == null) throw new NotFoundException($"Merchant not found for userId {userId}!");
        return merchant.Adapt<MerchantResponseDto>();
    }

    public async Task<IEnumerable<OfferDto>> GetOffersAsync(int userId, CancellationToken ct = default)
    {
        var merchant = await _merchantRepository.GetMerchantByUserIdAsync(userId, ct).ConfigureAwait(false);
        if (merchant == null) throw new NotFoundException($"Merchant not found for userId {userId}!");

        var offers = await _offerRepository.GetByMerchantIdAsync(merchant.Id, ct).ConfigureAwait(false);
        var offerDtos = offers.Adapt<List<OfferDto>>();

        var categoryIds = offers.Select(x => x.CategoryId).Distinct().ToList();
        var categories = await _categoryRepository.GetByIdsAsync(categoryIds, ct).ConfigureAwait(false);
        var categoryDict = categories.ToDictionary(x => x.Id, x => x.Name);
        foreach (var dto in offerDtos)
            if (categoryDict.TryGetValue(dto.CategoryId, out var categoryName)) dto.Category = categoryName;

        return offerDtos;
    }

    public async Task<IEnumerable<MerchantResponseDto>> GetAllMerchantsAsync(CancellationToken ct = default)
        => (await _merchantRepository.GetAllAsync(ct).ConfigureAwait(false)).Adapt<IEnumerable<MerchantResponseDto>>();

    public async Task AddMerchantAsync(CreateMerchantDto merchant, CancellationToken ct = default)
        => await _merchantRepository.AddAsync(merchant.Adapt<Merchant>(), ct).ConfigureAwait(false);
    

    public async Task UpdateMerchantAsync(UpdateMerchantDto dto, CancellationToken ct = default)
    {
        var merchant = await _merchantRepository.GetByIdAsync(dto.Id, ct).ConfigureAwait(false);
        if (merchant == null) throw new NotFoundException($"Merchant not found with id {dto.Id}!");
        await _merchantRepository.UpdateAsync(dto.Adapt(merchant), ct).ConfigureAwait(false);
    }

    public async Task<MerchantResponseDto?> GetMerchantByIdAsync(int id, CancellationToken ct = default)
    {
        var merchant = await _merchantRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (merchant == null) throw new NotFoundException($"Merchant with id {id} not found!");
        return merchant.Adapt<MerchantResponseDto>();
    }

    public async Task DeleteMerchantAsync(int id, CancellationToken ct = default)
    {
        var merchant = await _merchantRepository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (merchant == null) throw new NotFoundException($"Merchant with id {id} not found!");
        await _merchantRepository.DeleteAsync(merchant, ct).ConfigureAwait(false);
        await _userService.DeleteUserAsync(merchant.UserId, ct).ConfigureAwait(false);
    }
}
