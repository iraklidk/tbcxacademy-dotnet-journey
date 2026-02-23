using Mapster;
using MVC.Models.Merchant;
using MVC.Models.Customer;
using Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService) => _authService = authService;

    #region Auth&Register&Forgotpassword
    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(CustomerRegisterViewModel model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _authService.RegisterAsync(model.Adapt<RegisterRequest>(), ct).ConfigureAwait(false);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);

        var isValidUser = await _authService.LoginAsync(model, ct).ConfigureAwait(false);
        if (isValidUser is null)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }        

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync().ConfigureAwait(false);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> RegisterMerchant() => View();

    [HttpPost]
    public async Task<IActionResult> RegisterMerchant(MerchantRegisterViewModel model, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await _authService.RegisterAsync(model.Adapt<RegisterRequest>(), ct).ConfigureAwait(false);
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Email"] = email;
            return View();
        }

        await _authService.ForgotPasswordAsync(email, ct).ConfigureAwait(false);

        TempData["SuccessMessage"] = "Check your email for the reset link.";
        return RedirectToAction(nameof(Login));
    }
    #endregion

    [HttpGet]
    public IActionResult AccessDenied() => View();

    [HttpGet]
    [Authorize]
    public IActionResult Profile()
    {
        if (User.IsInRole("Admin")) return RedirectToAction("Index", "Admin");
        if (User.IsInRole("Merchant")) return RedirectToAction("Index", "Merchant");
        return RedirectToAction("Index", "Customer");
    }
}
