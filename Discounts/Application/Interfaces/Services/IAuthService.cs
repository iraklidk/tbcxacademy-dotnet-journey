using Application.DTOs.Auth;
using Application.DTOs.User;

namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task<UserDto?> ValidateUserAsync(string username, string password, CancellationToken ct = default);

    Task<LoginResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);

    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);

    Task ForgotPasswordAsync(string email, CancellationToken ct = default);
}
