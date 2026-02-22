using Mapster;
using Domain.Constants;
using MVC.Models.Offer;
using MVC.Models.Server;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly ICategoryService _categoryService;
        public HomeController(IOfferService offerService,
                              ICategoryService categoryService)
        {
            _offerService = offerService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> Offers(string searchTerm, int? categoryId, CancellationToken ct = default)
        {
            var offers = await _offerService.GetAllWithCategoryNamesAsync(ct).ConfigureAwait(false);
            var categories = await _categoryService.GetAllAsync(ct).ConfigureAwait(false);

            if (!User.IsInRole("Admin")) offers = offers.Where(o => o.Status == OfferStatus.Approved || o.Status == OfferStatus.Expired);
            if (!string.IsNullOrEmpty(searchTerm)) offers = offers.Where(o => o.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            if (categoryId.HasValue && categoryId.Value != 0) offers = offers.Where(o => o.CategoryId == categoryId.Value).ToList();

            ViewBag.Categories = categories;
            return View(offers.Adapt<IEnumerable<OfferViewModel>>());
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
