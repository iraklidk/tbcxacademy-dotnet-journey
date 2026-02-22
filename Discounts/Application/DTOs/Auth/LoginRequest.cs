namespace Application.DTOs.Auth;

public class LoginRequest
{
    public string UserName { get; init; } = null!;

    public string Password { get; init; } = null!;
}
