using Microsoft.AspNetCore.Authorization;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Auth;
using Persistence.Identity;
using MVC.Models.Customer;
using MVC.Models.Merchant;
using Mapster;

public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IAuthService _authService;

    public AccountController(SignInManager<User> signInManager,
                             UserManager<User> userManager,
                             IAuthService authService)
    {
        _signInManager = signInManager;
        _authService = authService;
        _userManager = userManager;
    }

    #region Auth&Register
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

        var isValidUser = await _authService.ValidateUserAsync(model.UserName, model.Password, ct).ConfigureAwait(false);
        if (isValidUser is null)
        {
            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
        await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync().ConfigureAwait(false);
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
    [Authorize]
    public IActionResult Profile()
    {
        if (User.IsInRole("Admin")) return RedirectToAction("Index", "Admin");
        if (User.IsInRole("Merchant")) return RedirectToAction("Index", "Merchant");
        return RedirectToAction("Index", "Customer");
    }
}
