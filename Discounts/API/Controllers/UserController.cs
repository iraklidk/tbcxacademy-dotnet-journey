using Application.DTOs.User;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.Identity;
using Swashbuckle.AspNetCore.Filters;

namespace Discounts.API.Controllers;

/// <summary>
/// Provides endpoints for managing users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

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
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Returns 200 if deletion is successful.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> DeleteUserById(int id, CancellationToken ct)
    {
        await _userService.DeleteUserAsync(id, ct).ConfigureAwait(false);
        return Ok();
    }
}
