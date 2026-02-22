namespace Application.DTOs.User;

public class UserDto
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public string Role { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;
}

// bool isBlocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.Now;
