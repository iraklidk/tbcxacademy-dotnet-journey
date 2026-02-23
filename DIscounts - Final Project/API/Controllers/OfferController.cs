using Application.DTOs.Offer;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using Application.Interfaces.Services;
using API.Infrastructure.SwaggerExamples;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

/// <summary>
/// Provides endpoints for managing offers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OfferController : ControllerBase
{
    private readonly IOfferService _offerService;

    public OfferController(IOfferService offerService) => _offerService = offerService;

    /// <summary>
    /// Get all offers.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of all offers.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OfferDto>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var offers = await _offerService.GetAllWithCategoryNamesAsync(ct).ConfigureAwait(false);
        return Ok(offers);
    }

    /// <summary>
    /// Get offer by identifier.
    /// </summary>
    /// <param name="id">Offer identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Offer details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OfferDto), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(OfferDtoExample))]
    [SwaggerRequestExample(typeof(CreateOfferDto), typeof(CreateOfferDtoExample))]
    [SwaggerResponseExample(201, typeof(OfferDtoExample))]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var offer = await _offerService.GetByIdAsync(id, ct).ConfigureAwait(false);
        return Ok(offer);
    }

    /// <summary>
    /// Retrieves all pending offers.
    /// </summary>
    /// <param name="ct">Optional cancellation token.</param>
    /// <returns>List of pending offers.</returns>
    [HttpGet("pendings")]
    [ProducesResponseType(typeof(IEnumerable<OfferDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendingsAsync(CancellationToken ct = default)
    {
        var pendingOffers = await _offerService.GetPendingsAsync(ct).ConfigureAwait(false);
        return Ok(pendingOffers);
    }

    /// <summary>
    /// Create a new offer.
    /// </summary>
    /// <param name="dto">Offer creation details.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created offer.</returns>
    [HttpPost]
    [Authorize(Roles = "Merchant")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(OfferDto), 201)]
    public async Task<IActionResult> Create([FromBody] CreateOfferDto dto, CancellationToken ct)
    {
        var createdOffer = await _offerService.CreateOfferAsync(dto, ct).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetById), new { id = createdOffer.Id }, createdOffer);
    }

    /// <summary>
    /// Updates the remaining coupon count for a specific offer.
    /// </summary>
    /// <param name="offerId">The ID of the offer to update.</param>
    /// <param name="count">The number of coupons to change (default is 1).</param>
    /// <param name="ct">Optional cancellation token.</param>
    /// <returns>Returns HTTP 204 No Content if successful.</returns>
    [HttpPost("{offerId}/change-coupons")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeRemainingCouponsAsync(int offerId, int count = 1, CancellationToken ct = default)
    {
        await _offerService.ChangeRemainingCouponsAsync(offerId, count, ct).ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Update an existing offer.
    /// </summary>
    /// <param name="dto">Updated offer details.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if update is successful.</returns>
    [HttpPut]
    [Authorize(Roles = "Merchant")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update([FromBody] UpdateOfferDto dto, CancellationToken ct)
    {
        await _offerService.UpdateOfferAsync(dto, ct).ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Delete an offer by identifier.
    /// </summary>
    /// <param name="id">Offer identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if deletion is successful.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Merchant")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _offerService.DeleteOfferAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }
}
