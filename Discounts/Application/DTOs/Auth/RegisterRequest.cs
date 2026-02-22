namespace Application.DTOs.Auth;

public class RegisterRequest
{
    public string Email { get; init; } = null!;

    public string? Name { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string PhoneNumber { get; init; } = null!;

    public string Password { get; init; } = null!;

    public string UserName { get; init; } = null!;

    public string Role { get; init; } = null!;
}
