namespace Application.DTOs.User;

public class UpdateUserDto
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string? Password { get; set; }

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;
}
