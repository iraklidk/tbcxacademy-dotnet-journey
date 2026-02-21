using Discounts.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Repos;
using Application.DTOs.User;
using Domain.Entities;

namespace Discounts.Persistence.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(DiscountsDbContext context) : base(context) { }

    public async Task<Customer?> GetCustomerByUserIdAsync(int userId, CancellationToken ct = default)
        => (await _context.Customers.FirstOrDefaultAsync(u => u.UserId == userId, ct).ConfigureAwait(false));

    public async Task<IEnumerable<UserDto?>> GetAllUsersWithRolesAsync(CancellationToken ct = default)
        => await _context.Users.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                Role = (from userRole in _context.UserRoles
                        join role in _context.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId == user.Id
                        select role.Name).FirstOrDefault()
            })
            .ToListAsync(ct).ConfigureAwait(false);
}
