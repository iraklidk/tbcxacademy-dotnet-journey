using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Application.DTOs.GlobalSettings;
using Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using API.Infrastructure.SwaggerExamples;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

/// <summary>
/// Provides endpoints for managing global settings.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GlobalSettingsController : ControllerBase
{
    private readonly IGlobalSettingsService _globalSettingsService;
    public GlobalSettingsController(IGlobalSettingsService globalSettingsService)
        => _globalSettingsService = globalSettingsService;

    /// <summary>
    /// Retrieves the current global settings.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The current <see cref="GlobalSettingsDto"/>.</returns>
    /// <response code="200">Returns the global settings.</response>
    /// <response code="404">If the global settings are not found.</response>
    [HttpGet]
    [SwaggerResponse(200, "Returns the global settings", typeof(GlobalSettingsDto))]
    [SwaggerResponse(404, "Global settings not found")]
    public async Task<ActionResult<GlobalSettingsDto>> Get(CancellationToken ct)
    {
        var settings = await _globalSettingsService.GetSettingsAsync(ct).ConfigureAwait(false);
        return Ok(settings);
    }

    /// <summary>
    /// Updates the global settings.
    /// </summary>
    /// <param name="dto">The updated global settings.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content on success.</returns>
    /// <response code="204">Settings successfully updated.</response>
    /// <response code="404">If the global settings are not found.</response>
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [SwaggerRequestExample(typeof(UpdateGlobalSettingsDto), typeof(UpdateGlobalSettingsDtoExample))]
    [SwaggerResponse(204, "Settings successfully updated")]
    [SwaggerResponse(404, "Global settings not found")]
    public async Task<IActionResult> Update([FromBody] UpdateGlobalSettingsDto dto, CancellationToken ct)
    {
        await _globalSettingsService.UpdateSettingsAsync(dto, ct).ConfigureAwait(false);
        return NoContent();
    }
}
