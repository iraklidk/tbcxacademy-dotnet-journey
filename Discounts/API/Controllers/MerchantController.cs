using Domain.Entities;
using Application.DTOs.Offer;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Merchant;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

/// <summary>
/// Provides endpoints for managing merchants.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MerchantController : ControllerBase
{
    private readonly IMerchantService _merchantService;
    public MerchantController(IMerchantService merchantService) => _merchantService = merchantService;

    /// <summary>
    /// Gets merchant by id
    /// </summary>
    /// <param name="id">Merchant identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Merchant details</returns>
    /// <response code="200">Merchant found</response>
    /// <response code="404">Merchant not found</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MerchantResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _merchantService.GetMerchantByIdAsync(id, ct).ConfigureAwait(false);
        return Ok(result);
    }

    /// <summary>
    /// Gets merchant by user id
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Merchant details</returns>
    /// <response code="200">Merchant found</response>
    /// <response code="404">Merchant not found</response>
    [HttpGet("by-user/{userId:int}")]
    [ProducesResponseType(typeof(MerchantResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(int userId, CancellationToken ct)
    {
        var result = await _merchantService.GetMerchantByUserIdAsync(userId, ct).ConfigureAwait(false);
        return Ok(result);
    }

    /// <summary>
    /// Gets all merchants
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of merchants</returns>
    /// <response code="200">Merchants retrieved successfully</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Merchant>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _merchantService.GetAllMerchantsAsync(ct).ConfigureAwait(false);
        return Ok(result);
    }

    /// <summary>
    /// Gets all offers for a merchant by user id
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of offers</returns>
    /// <response code="200">Offers retrieved successfully</response>
    [HttpGet("{userId:int}/offers")]
    [ProducesResponseType(typeof(IEnumerable<OfferDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOffers(int userId, CancellationToken ct)
    {
        var result = await _merchantService.GetOffersAsync(userId, ct).ConfigureAwait(false);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new merchant
    /// </summary>
    /// <param name="merchant">Merchant object</param>
    /// <param name="ct">Cancellation token</param>
    /// <response code="201">Merchant created successfully</response>
    /// <response code="400">Invalid request</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateMerchantDto merchant, CancellationToken ct)
    {
        await _merchantService.AddMerchantAsync(merchant, ct).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetByUserId), new { userId = merchant.UserId }, merchant);
    }

    /// <summary>
    /// Updates an existing merchant
    /// </summary>
    /// <param name="merchant">Merchant object</param>
    /// <param name="ct">Cancellation token</param>
    /// <response code="204">Merchant updated successfully</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromBody] UpdateMerchantDto merchant, CancellationToken ct)
    {
        await _merchantService.UpdateMerchantAsync(merchant, ct).ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Deletes a merchant by id
    /// </summary>
    /// <param name="id">Merchant identifier</param>
    /// /// <param name="ct">Cancellation token to cancel the operation if needed</param>
    /// <response code="204">Merchant deleted successfully</response>
    /// <response code="404">Merchant not found</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _merchantService.DeleteMerchantAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }
}
