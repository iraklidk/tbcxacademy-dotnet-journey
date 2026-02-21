using Discounts.Application.Exceptions;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Exceptions.User;
using Application.DTOs.Merchant;
using Application.DTOs.Customer;
using Application.Interfaces;
using Application.DTOs.User;
using Application.DTOs.Auth;
using Persistence.Identity;
using Domain.Entities;
using System.Data;
using Mapster;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly IMerchantService _merchantService;
    private readonly ICustomerService _customerService;
    private readonly UserManager<User> _userManager;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(SignInManager<User> signInManager,
                       IMerchantService merchantService,
                       ICustomerService customerService,
                       UserManager<User> userManager,
                       IEmailService emailService,
                       IUnitOfWork unitOfWork)
    {
        _merchantService = merchantService;
        _customerService = customerService;
        _signInManager = signInManager;
        _emailService = emailService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> ValidateUserAsync(string username, string password, CancellationToken ct)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username, ct).ConfigureAwait(false);
        if (!(await _userManager.Users.AnyAsync(u => u.UserName == username, ct).ConfigureAwait(false)))
            throw new UserNotFound($"User {username} doesn't exists!");
        if (!user.IsActive) throw new DomainException("User account is blocked!");
        var result = await _signInManager.PasswordSignInAsync(username, password, true, false).ConfigureAwait(false);
        return result.Succeeded ? user.Adapt<UserDto>() : null;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName, ct).ConfigureAwait(false);
        if (user is null) throw new UserNotFound($"{request.UserName} doesn’t seem to be registered!");
        if (!(await _userManager.CheckPasswordAsync(user, request.Password).ConfigureAwait(false))) throw new ValidationException("Invalid password!");
        if (!user.IsActive) throw new DomainException("User account is blocked!");

        var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        return new LoginResponse
        {
            UserName = user.UserName,
            UserId = user.Id,
        };
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct).ConfigureAwait(false);

        var emailExists = await _userManager.Users.AnyAsync(u => u.Email == request.Email, ct).ConfigureAwait(false);
        var usernameExists = await _userManager.Users.AnyAsync(u => u.UserName == request.UserName, ct).ConfigureAwait(false);

        if (emailExists || usernameExists)
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
        return new LoginResponse
        {
            UserName = user.UserName,
            UserId = user.Id,
        };
    }

    public async Task ForgotPasswordAsync(string email, CancellationToken ct = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email, ct).ConfigureAwait(false);
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
