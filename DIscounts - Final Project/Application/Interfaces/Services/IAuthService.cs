using Application.DTOs.Auth;

namespace Application.Interfaces.Services;

public interface IAuthService
{
    Task LogoutAsync();

    Task ForgotPasswordAsync(string email, CancellationToken ct = default);

    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);

    Task<LoginResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
}
