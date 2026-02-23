using Asp.Versioning;
using Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Application.Interfaces.Services;
using API.Infrastructure.SwaggerExamples;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace Discounts.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>
    /// Authenticate user with username and password.
    /// </summary>
    /// <param name="request">Login credentials.</param>
    /// <param name="ct">Cancellation token.</param>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [SwaggerResponseExample(200, typeof(LoginResponseExample))]
    [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct = default)
    {
        var response = await _authService.LoginAsync(request, ct).ConfigureAwait(false);
        return Ok(response);
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    /// <returns>A message confirming logout.</returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync().ConfigureAwait(false);
        return Ok(new { message = "Logged out successfully" });
    }

    /// <summary>
    /// Register a new user account.
    /// </summary>
    /// <param name="request">User registration details.</param>
    /// <param name="ct">Cancellation token.</param>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [SwaggerResponseExample(201, typeof(LoginResponseExample))]
    [SwaggerRequestExample(typeof(RegisterRequest), typeof(RegisterRequestExample))]
    public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request,
        CancellationToken ct = default)
    {
        var response = await _authService.RegisterAsync(request, ct).ConfigureAwait(false);
        return Ok(response);
    }

    /// <summary>
    /// Send password reset instructions to the provided email address.
    /// </summary>
    /// <param name="email">User email address.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if the request is processed successfully.</returns>
    [AllowAnonymous]
    [ProducesResponseType(204)]
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody][Required][EmailAddress] string email, CancellationToken ct = default)
    {
        await _authService.ForgotPasswordAsync(email, ct).ConfigureAwait(false);
        return NoContent();
    }
}
