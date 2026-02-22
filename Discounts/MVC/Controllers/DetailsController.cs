using Mapster;
using MVC.Models.Offer;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;

namespace Discounts.Web.Controllers
{
    public class DetailsController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IMerchantService _merchantService;

        public DetailsController(IOfferService offerService,
                                 IMerchantService merchantService)
        {
            _offerService = offerService;
            _merchantService = merchantService;
        }

        [HttpGet("/Details/{id}")]
        public async Task<IActionResult> OfferDetails(int id, CancellationToken ct = default)
        {
            var offer = await _offerService.GetByIdAsync(id, ct).ConfigureAwait(false);
            var model = offer.Adapt<OfferViewModel>();

            if (User.IsInRole("Admin")) return View("DetailsAdmin", model);
            else if (User.IsInRole("Merchant"))
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var merchant = await _merchantService.GetMerchantByUserIdAsync(userId, ct).ConfigureAwait(false);
                ViewBag.OfferMerchantId = merchant.Id;
                ViewBag.MerchantId = offer.MerchantId;
                return View("DetailsMerchant", model);
            }
            else if (User.IsInRole("Customer"))
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                model.IsReserved = await _offerService.IsOfferReservedByUserAsync(model.Id, userId, ct).ConfigureAwait(false);
                return View("DetailsUser", model);
            }
            else { return View("DetailsAnonymous", model); }
        }
    }
}
