using Application.DTOs.User;
using Domain.Entities;

namespace Application.Interfaces.Repos;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<IEnumerable<UserDto?>> GetAllUsersWithRolesAsync(CancellationToken ct = default);

    Task<Customer?> GetCustomerByUserIdAsync(int userId, CancellationToken ct = default);
}
