using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Offer;
using System.Security.Claims;
using MVC.Models.Offer;
using Mapster;

[Authorize(Roles = "Merchant")]
public class MerchantController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IMerchantService _merchantService;
    private readonly ICouponService _couponService;
    private readonly IOfferService _offerService;

    public MerchantController(ICategoryService categoryService,
                              IMerchantService merchantService,
                              ICouponService couponApiService,
                              IOfferService offerService)
    {
        _categoryService = categoryService;
        _merchantService = merchantService;
        _couponService = couponApiService;
        _offerService = offerService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        ViewBag.Balance = (await _merchantService.GetMerchantByUserIdAsync(userId, ct).ConfigureAwait(false)).Balance;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> SalesHistory(CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var coupons = await _couponService.GetCouponsByMerchantAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), ct).ConfigureAwait(false);
        return View(coupons);
    }

    #region Offers
    [HttpGet]
    public async Task<IActionResult> Offers(CancellationToken ct = default)
    {
        var categories = await _categoryService.GetAllAsync(ct).ConfigureAwait(false);
        ViewBag.Categories = categories;
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var offers = await _merchantService.GetOffersAsync(userId, ct).ConfigureAwait(false);
        var offersDto = offers.Adapt<List<OfferViewModel>>();
        return View(offersDto);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteOffer(int id, CancellationToken ct = default)
    {
        await _offerService.DeleteOfferAsync(id, ct).ConfigureAwait(false);
        return RedirectToAction(nameof(Offers));
    }

    [HttpGet]
    public IActionResult AddOffer() => View();

    [HttpPost]
    public async Task<IActionResult> AddOffer(CreateOfferDto model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        await _offerService.CreateOfferAsync(model, ct).ConfigureAwait(false);
        return RedirectToAction(nameof(Offers));
    }

    [HttpPost]
    public async Task<IActionResult> EditOffer(UpdateOfferDto model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return RedirectToAction(nameof(Offers));
        await _offerService.UpdateOfferAsync(model, ct).ConfigureAwait(false);
        return RedirectToAction(nameof(Offers));
    }
    #endregion
}
