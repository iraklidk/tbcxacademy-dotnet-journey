using Mapster;
using Persistence.Identity;
using Application.DTOs.User;
using Application.Interfaces;
using Application.Exceptions.User;
using Application.Interfaces.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces.Services;
using Discounts.Application.Exceptions;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly ICustomerRepository _customerRepository;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public UserService(IUnitOfWork unitOfWork,
                       UserManager<User> userManager,
                       ICustomerRepository customerRepository,
                       RoleManager<IdentityRole<int>> roleManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _roleManager = roleManager;
        _customerRepository = customerRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken ct = default)
        => (await _userManager.Users.ToListAsync(ct).ConfigureAwait(false)).Adapt<List<UserDto>>();

    public async Task<User?> GetUserByIdAsync(int id, CancellationToken ct)
        => await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false)
        ?? throw new UserNotFound($"User with id {id} not found!");

    public async Task<IEnumerable<UserDto>> GetBatchUsersAsync(List<int> ids, CancellationToken ct)
    {
        if (ids == null || !ids.Any()) return Enumerable.Empty<UserDto>();
        return (await _userManager.Users.Where(u => ids.Contains(u.Id)).ToListAsync(ct).ConfigureAwait(false)).Adapt<IEnumerable<UserDto>>();
    }

    public async Task UpdateUserAsync(UpdateUserDto dto, CancellationToken ct = default)
    {
        await _unitOfWork.BeginTransactionAsync(ct).ConfigureAwait(false);

        var result = new IdentityResult();
        var entity = await _userManager.FindByIdAsync(dto.Id.ToString()).ConfigureAwait(false);
        if (entity == null) throw new UserNotFound($"User with id {dto.Id} not found!");

        if (dto.Password != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(entity).ConfigureAwait(false);
            await _userManager.ResetPasswordAsync(entity, token, dto.Password).ConfigureAwait(false);
        }
        else { dto.Password = entity.PasswordHash; }

        result = await _userManager.UpdateAsync(dto.Adapt(entity)).ConfigureAwait(false);
        var role = dto.RoleId switch
        {
            1 => "Admin",
            2 => "Merchant",
            3 => "Customer",
            _ => throw new DomainException($"Invalid role id {dto.RoleId}!")
        };
        var currentRoles = await _userManager.GetRolesAsync(entity).ConfigureAwait(false);
        await _userManager.RemoveFromRolesAsync(entity, currentRoles).ConfigureAwait(false);
        await _userManager.AddToRoleAsync(entity, role).ConfigureAwait(false);

        await _unitOfWork.CommitAsync(ct).ConfigureAwait(false);
    }

    public async Task DeleteUserAsync(int id, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        if (user == null) throw new UserNotFound($"User with id {id} not found!");
        await _userManager.DeleteAsync(user).ConfigureAwait(false);
    }

    public async Task ChangeUserStatusAsync(int id, CancellationToken ct = default)
    {
        var entity = await _userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
        if (entity == null) throw new UserNotFound($"User with id {id} not found!");
        entity.IsActive = !entity.IsActive;
        await _userManager.UpdateAsync(entity).ConfigureAwait(false);
    }

    public Task<IEnumerable<UserDto?>> GetAllUsersWithRolesAsync(CancellationToken ct = default)
        => _customerRepository.GetAllUsersWithRolesAsync(ct);
}
