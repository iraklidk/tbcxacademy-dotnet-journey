using Application.DTOs.User;
using Persistence.Identity;

namespace Application.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetBatchUsersAsync(List<int> ids, CancellationToken ct = default);

    Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken ct = default);

    Task UpdateUserAsync(UpdateUserDto dto, CancellationToken ct = default);

    Task<User?> GetUserByIdAsync(int id, CancellationToken ct = default);

    Task<IEnumerable<UserDto?>> GetAllUsersWithRolesAsync(CancellationToken ct);

    Task ChangeUserStatusAsync(int id, CancellationToken ct = default);

    Task DeleteUserAsync(int id, CancellationToken ct = default);
}
