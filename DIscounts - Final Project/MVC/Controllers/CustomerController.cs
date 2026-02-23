using Mapster;
using Domain.Constants;
using MVC.Models.Offer;
using System.Security.Claims;
using Application.DTOs.Coupon;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Reservation;
using Application.Interfaces.Services;
using Discounts.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace MVC.Controllers;

[Authorize(Roles = "Customer")]
public class CustomerController : Controller
{
    private readonly IOfferService _offerService;
    private readonly ICouponService _couponService;
    private readonly ICustomerService _customerService;
    private readonly ICategoryService _categoryService;
    private readonly IReservationService _reservationService;

    public CustomerController(IOfferService offerService,
                              ICouponService couponService,
                              ICategoryService categoryService,
                              ICustomerService customerService,
                              IReservationService reservationService)
    {
        _offerService = offerService;
        _couponService = couponService;
        _customerService = customerService;
        _categoryService = categoryService;
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> Browse(string searchTerm, int? categoryId, CancellationToken ct = default)
    {
        var offers = await _offerService.GetAllWithCategoryNamesAsync(ct).ConfigureAwait(false);
        var categories = await _categoryService.GetAllAsync(ct).ConfigureAwait(false);
        ViewBag.Categories = categories;
        var filteredOffers = offers.Where(o => o.Status == OfferStatus.Approved || o.Status == OfferStatus.Expired);
        if (!string.IsNullOrEmpty(searchTerm)) filteredOffers = offers.Where(o => o.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        if (categoryId.HasValue && categoryId.Value != 0) offers = offers.Where(o => o.CategoryId == categoryId.Value);
        var offerViewModels = filteredOffers.Adapt<IEnumerable<OfferViewModel>>().ToList();
        ViewBag.Categories = categories;
        return View(offers.Adapt<IEnumerable<OfferViewModel>>());
    }

    [HttpGet]
    public async Task<IActionResult> MyCoupons(CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var coupons = await _couponService.GetByUserAsync(userId, ct).ConfigureAwait(false);
        return View(coupons);
    }

    [HttpGet]
    public async Task<IActionResult> Reservations(CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var reservations = await _reservationService.GetByUserAsync(userId, ct).ConfigureAwait(false);
        return View(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        ViewBag.Balance = (await _customerService.GetCustomerByUserIdAsync(userId, ct).ConfigureAwait(false)).Balance;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Reserve(int offerId, CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var dto = new CreateReservationDto { OfferId = offerId, UserId = userId, ExpiresAt = DateTime.Now.AddMinutes(30) };
        await _reservationService.CreateReservationAsync(dto, ct).ConfigureAwait(false);
        return RedirectToAction("OfferDetails", "Details", new { id = offerId });
    }

    [HttpPost]
    public async Task<IActionResult> Buy(int offerId, CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        await _couponService.CreateCouponAsync(new CreateCouponDto { UserId = userId, OfferId = offerId }, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "Your coupon has been generated and added to your profile.";
        return RedirectToAction("OfferDetails", "Details", new { id = offerId });
    }

    [HttpPost]
    public async Task<IActionResult> BuyFromReservation(decimal price, int offerId, int reservationId, CancellationToken ct = default)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var customer = await _customerService.GetCustomerByUserIdAsync(userId, ct).ConfigureAwait(false);
        if (customer.Balance >= price)
        {
            var reservation = await _reservationService.GetByIdAsync(reservationId, ct).ConfigureAwait(false);
            await _reservationService.DeleteReservationAsync(reservation.Id, ct).ConfigureAwait(false);
            await _offerService.ChangeRemainingCouponsAsync(offerId, 1, ct).ConfigureAwait(false);
            await _couponService.CreateCouponAsync(new CreateCouponDto { UserId = userId, OfferId = offerId }, ct).ConfigureAwait(false);
        }
        else { throw new DomainException("Not Enough Money"); }
        TempData["SuccessMessage"] = "Your coupon has been generated and added to your profile.";
        return RedirectToAction("Reservations", "Customer");
    }
}
