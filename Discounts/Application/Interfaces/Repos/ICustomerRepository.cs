using Application.DTOs.User;
using Domain.Entities;

namespace Application.Interfaces.Repos;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer?> GetCustomerByUserIdAsync(int userId, CancellationToken ct = default);

    Task<IEnumerable<UserDto?>> GetAllUsersWithRolesAsync(CancellationToken ct = default);
}
