using Mapster;
using System.Data;
using Domain.Entities;
using Persistence.Identity;
using Application.DTOs.Auth;
using Application.Interfaces;
using Application.DTOs.Customer;
using Application.DTOs.Merchant;
using Application.Exceptions.User;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces.Services;
using Discounts.Application.Exceptions;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly UserManager<User> _userManager;
    private readonly ICustomerService _customerService;
    private readonly IMerchantService _merchantService;
    private readonly SignInManager<User> _signInManager;

    public AuthService(IUnitOfWork unitOfWork,
                       IEmailService emailService,
                       UserManager<User> userManager,
                       ICustomerService customerService,
                       IMerchantService merchantService,
                       SignInManager<User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _emailService = emailService;
        _signInManager = signInManager;
        _customerService = customerService;
        _merchantService = merchantService;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
        if (user is null) throw new UserNotFound($"{request.UserName} doesn’t seem to be registered!");
        if (!(await _userManager.CheckPasswordAsync(user, request.Password).ConfigureAwait(false))) throw new ValidationException("Invalid Password!");
        if (!user.IsActive) throw new DomainException("User account is blocked!");
        await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
        return new LoginResponse { UserName = user.UserName, UserId = user.Id };
    }

    public Task LogoutAsync() => _signInManager.SignOutAsync();

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct).ConfigureAwait(false);

        var existingUser = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
        var usernameExists = await _userManager.FindByNameAsync(request.UserName).ConfigureAwait(false);
        if (existingUser is not null || usernameExists is not null)
        {
            await _unitOfWork.RollbackAsync(ct).ConfigureAwait(false);
            throw new UserAlreadyExists($"{request.Email} or {request.UserName} is already registered!");
        }

        var user = request.Adapt<User>();
        var result = await _userManager.CreateAsync(user, request.Password).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            await _unitOfWork.RollbackAsync(ct).ConfigureAwait(false);
            throw new DomainException($"Failed to register user: {string.Join(", ", result.Errors.Select(e => e.Description))}!");
        }

        if (request.Role == "Customer")
        {
            var customer = request.Adapt<CreateCustomerDto>();
            customer.UserId = user.Id;
            await _customerService.AddCustomerAsync(customer, ct).ConfigureAwait(false);
        }
        else
        {
            var merchant = request.Adapt<Merchant>();
            merchant.UserId = user.Id;
            await _merchantService.AddMerchantAsync(merchant.Adapt<CreateMerchantDto>(), ct).ConfigureAwait(false);
        }

        var roleResult = await _userManager.AddToRoleAsync(user, request.Role).ConfigureAwait(false);
        if (!roleResult.Succeeded)
        {
            await _unitOfWork.RollbackAsync(ct).ConfigureAwait(false);
            throw new DomainException($"Failed to assign role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}!");
        }

        await _unitOfWork.CommitAsync(ct).ConfigureAwait(false);
        await _emailService.SendRegistrationConfirmedAsync(request.Email, ct).ConfigureAwait(false);
        return new LoginResponse { UserName = user.UserName, UserId = user.Id, };
    }

    public async Task ForgotPasswordAsync(string email, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);
        if (user is null) throw new UserNotFound($"User not found with email {email}!");
        var newPassword = Guid.NewGuid().ToString().Substring(0, 8);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword).ConfigureAwait(false);
        if (!result.Succeeded) throw new(string.Join(", ", result.Errors.Select(e => e.Description)));
        var subject = "Password Reset";
        var message = $"Your new password is: {newPassword}";
        await _emailService.NotifyUserAsync(email, subject, message, ct).ConfigureAwait(false);
    }
}
