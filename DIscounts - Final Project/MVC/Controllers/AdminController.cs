using Mapster;
using Application.DTOs.User;
using Application.DTOs.Offer;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Category;
using Application.Interfaces.Services;
using Application.DTOs.GlobalSettings;
using Discounts.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserService _userService;
    private readonly IOfferService _offerService;
    private readonly ICategoryService _categoryService;
    private readonly IGlobalSettingsService _globalSettingsService;

    public AdminController(IUserService userService,
                           IOfferService offerService,
                           ICategoryService categoryService,
                           IGlobalSettingsService globalSettingsService)
    {
        _userService = userService;
        _offerService = offerService;
        _categoryService = categoryService;
        _globalSettingsService = globalSettingsService;
    }

    public IActionResult Index() => View();

    #region User Management
    [HttpGet]
    public async Task<IActionResult> Users(CancellationToken ct = default)
    {
        var users = await _userService.GetAllUsersWithRolesAsync(ct).ConfigureAwait(false);
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(UpdateUserDto model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            throw new DomainException("Error updating user!");
        }
        await _userService.UpdateUserAsync(model, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "User account edited successfully!";
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(int id, CancellationToken ct = default)
    {
        await _userService.DeleteUserAsync(id, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "User deleted!";
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeUserStatus(int id, CancellationToken ct = default)
    {
        await _userService.ChangeUserStatusAsync(id, ct).ConfigureAwait(false);
        return RedirectToAction(nameof(Users));
    }
    #endregion

    #region Global Settings
    [HttpGet]
    public async Task<IActionResult> Settings(CancellationToken ct = default)
    {
        var settings = await _globalSettingsService.GetSettingsAsync(ct).ConfigureAwait(false);
        return View(settings);
    }

    [HttpPost]
    public async Task<IActionResult> Settings(UpdateGlobalSettingsDto model, CancellationToken ct = default)
    {
        var updateDto = model.Adapt<UpdateGlobalSettingsDto>();
        await _globalSettingsService.UpdateSettingsAsync(updateDto, ct).ConfigureAwait(false);
        return View();
    }
    #endregion

    #region Category Management
    [HttpGet]
    public IActionResult Categories(CancellationToken ct = default)
    {
        var categories = _categoryService.GetAllAsync(ct).Result;
        return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(CreateCategoryDto categoryDto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetAllAsync(ct).ConfigureAwait(false);
            return View("Categories", categories);
        }
        await _categoryService.CreateAsync(categoryDto, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "Category added successfully!";
        return RedirectToAction(nameof(Categories));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct = default)
    {
        await _categoryService.DeleteAsync(id, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "Category deleted successfully!";
        return RedirectToAction(nameof(Categories));
    }

    [HttpPost]
    public async Task<IActionResult> EditCategory(UpdateCategoryDto dto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetAllAsync(ct).ConfigureAwait(false);
            return View("Categories", categories);
        }
        await _categoryService.UpdateAsync(dto, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "Category updated successfully!";
        return RedirectToAction(nameof(Categories));
    }
    #endregion

    #region Pending Discounts
    [HttpGet]
    public async Task<IActionResult> DiscountsPending(CancellationToken ct = default)
    {
        var pendingDiscounts = await _offerService.GetPendingsAsync(ct).ConfigureAwait(false);
        return View(pendingDiscounts);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(UpdateOfferStatusDto viewModel, bool fromDetailsPage, CancellationToken ct = default)
    {
        await _offerService.UpdateStatusAsync(viewModel, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "Offer status updated successfully!";
        return fromDetailsPage ? RedirectToAction("OfferDetails", "Details", new { id = viewModel.Id }) : RedirectToAction(nameof(DiscountsPending));
    }
    #endregion

    [HttpPost]
    public async Task<IActionResult> DeleteOffer(int offerId, CancellationToken ct = default)
    {
        await _offerService.DeleteOfferAsync(offerId, ct).ConfigureAwait(false);
        TempData["SuccessMessage"] = "Offer deleted successfully!";
        return RedirectToAction("Offers", "Home");
    }
}
