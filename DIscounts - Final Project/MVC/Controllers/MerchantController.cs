using System.Security.Claims;
using Application.DTOs.Offer;
using Application.Interfaces.Services;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.Offer;

[Authorize(Roles = "Merchant")]
public class MerchantController : Controller
{
    private readonly IOfferService _offerService;
    private readonly ICouponService _couponService;
    private readonly IMerchantService _merchantService;
    private readonly ICategoryService _categoryService;

    public MerchantController(IOfferService offerService,
                              ICouponService couponService,
                              IMerchantService merchantService,
                              ICategoryService categoryService)
    {
        _offerService = offerService;
        _couponService = couponService;
        _merchantService = merchantService;
        _categoryService = categoryService;
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
        var coupons = await _couponService.GetCouponsByMerchantAsync(userId, ct).ConfigureAwait(false);
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
        TempData["SuccessMessage"] = "The offer was deleted successfully.";
        return RedirectToAction(nameof(Offers));
    }

    [HttpGet]
    public IActionResult AddOffer() => View();

    [HttpPost]
    public async Task<IActionResult> AddOffer(CreateOfferDto model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        await _offerService.CreateOfferAsync(model, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "Offer added successfully. Pending admin approval.";
        return RedirectToAction(nameof(Offers));
    }

    [HttpPost]
    public async Task<IActionResult> EditOffer(UpdateOfferDto model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return RedirectToAction(nameof(Offers));
        await _offerService.UpdateOfferAsync(model, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "Offer updated successfully. Waiting for admin approval.";
        return RedirectToAction(nameof(Offers));
    }
    #endregion
}
