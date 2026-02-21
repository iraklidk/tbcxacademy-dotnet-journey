using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MVC.Models.Offer;
using Mapster;

namespace Discounts.Web.Controllers
{
    public class DetailsController : Controller
    {
        private readonly IMerchantService _merchantService;
        private readonly IOfferService _offerService;

        public DetailsController(IMerchantService merchantService,
                                 IOfferService offerService)
        {
            _merchantService = merchantService;
            _offerService = offerService;
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
