using static API.Infrastructure.SwaggerExamples.ReservationExamples;
using Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Filters;
using Application.DTOs.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Provides endpoints for managing reservations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    /// <summary>
    /// Get all reservations.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of all reservations.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ReservationDto>), 200)]
    public async Task<ActionResult<List<ReservationDto>>> GetAll(CancellationToken ct)
    {
        var reservations = await _reservationService.GetAllAsync(ct).ConfigureAwait(false);
        return Ok(reservations);
    }

    /// <summary>
    /// Get reservation by identifier.
    /// </summary>
    /// <param name="id">Reservation identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Reservation details.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReservationDto), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(ReservationDtoExample))]
    public async Task<ActionResult<ReservationDto>> GetById(int id, CancellationToken ct)
    {
        var reservation = await _reservationService.GetByIdAsync(id, ct).ConfigureAwait(false);
        return Ok(reservation);
    }

    /// <summary>
    /// Get all reservations for a specific customer.
    /// </summary>
    /// <param name="customerId">Customer identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of reservations associated with the customer.</returns>
    [HttpGet("customer/{customerId:int}")]
    [ProducesResponseType(typeof(List<ReservationDto>), 200)]
    public async Task<ActionResult<List<ReservationDto>>> GetByCustomer(int customerId, CancellationToken ct)
    {
        var reservations = await _reservationService.GetByCustomerAsync(customerId, ct).ConfigureAwait(false);
        return Ok(reservations);
    }

    /// <summary>
    /// Get all reservations for a specific offer.
    /// </summary>
    /// <param name="offerId">Offer identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of reservations associated with the offer.</returns>
    [HttpGet("offer/{offerId:int}")]
    [ProducesResponseType(typeof(List<ReservationDto>), 200)]
    public async Task<ActionResult<List<ReservationDto>>> GetByOffer(int offerId, CancellationToken ct)
    {
        var reservations = await _reservationService.GetByOfferAsync(offerId, ct).ConfigureAwait(false);
        return Ok(reservations);
    }

    /// <summary>
    /// Create a new reservation.
    /// </summary>
    /// <param name="dto">Reservation details.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created reservation.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ReservationDto), 201)]
    [ProducesResponseType(400)]
    [SwaggerResponseExample(201, typeof(ReservationDtoExample))]
    public async Task<ActionResult<ReservationDto>> Create([FromBody] CreateReservationDto dto, CancellationToken ct)
    {
        var created = await _reservationService.CreateReservationAsync(dto, ct).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Update an existing reservation.
    /// </summary>
    /// <param name="dto">Updated reservation details.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if update is successful.</returns>
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [SwaggerRequestExample(typeof(UpdateReservationDto), typeof(UpdateReservationDtoExample))]
    public async Task<IActionResult> Update([FromBody] UpdateReservationDto dto, CancellationToken ct)
    {
        await _reservationService.UpdateReservationAsync(dto, ct).ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Delete a reservation by identifier.
    /// </summary>
    /// <param name="id">Reservation identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if deletion is successful.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _reservationService.DeleteReservationAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }
}
