using Persistence.Identity;
using Application.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace Discounts.API.Controllers;

/// <summary>
/// Provides endpoints for managing users.
/// </summary>
[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService) => _userService = userService;

    /// <summary>
    /// Get user by identifier.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>User details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(UserDto))]
    public async Task<ActionResult<User>> GetUserById(int id, CancellationToken ct)
    {
        var user = await _userService.GetUserByIdAsync(id, ct).ConfigureAwait(false);
        return Ok(user);
    }

    /// <summary>
    /// Retrieves all users along with their assigned roles.
    /// </summary>
    /// <param name="ct">Optional cancellation token.</param>
    /// <returns>List of users with roles.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto?>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsersWithRolesAsync(CancellationToken ct = default)
    {
        var users = await _userService.GetAllUsersWithRolesAsync(ct).ConfigureAwait(false);
        return Ok(users);
    }

    /// <summary>
    /// Get multiple users by a batch of identifiers.
    /// </summary>
    /// <param name="userIds">List of user identifiers.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of users matching the provided identifiers.</returns>
    [HttpPost("batch")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUsersByBatch([FromBody] List<int> userIds, CancellationToken ct)
    {
        var users = await _userService.GetBatchUsersAsync(userIds, ct).ConfigureAwait(false);
        return Ok(users);
    }

    /// <summary>
    /// Toggles the active status of a specific user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="ct">Optional cancellation token.</param>
    /// <returns>Returns HTTP 204 No Content if successful.</returns>
    [HttpPost("{id}/toggle-status")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangeUserStatusAsync(int id, CancellationToken ct = default)
    {
        await _userService.ChangeUserStatusAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Returns 200 if deletion is successful.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<User>> DeleteUserById(int id, CancellationToken ct)
    {
        await _userService.DeleteUserAsync(id, ct).ConfigureAwait(false);
        return Ok();
    }
}
