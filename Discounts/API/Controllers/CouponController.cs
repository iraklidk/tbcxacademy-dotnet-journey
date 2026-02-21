using API.Infrastructure.SwaggerExamples;
using Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Coupon;

namespace API.Controllers;

/// <summary>
/// Provides endpoints for managing coupons.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CouponController : ControllerBase
{
    private readonly ICouponService _couponService;

    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }

    /// <summary>
    /// Get coupon by identifier.
    /// </summary>
    /// <param name="id">Coupon identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Coupon details.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(CouponDto), 200)]
    [SwaggerResponseExample(200, typeof(CouponDtoExample))]
    public async Task<ActionResult<CouponDto>> GetById(int id, CancellationToken ct)
    {
        var coupon = await _couponService.GetByIdAsync(id, ct).ConfigureAwait(false);
        return Ok(coupon);
    }

    /// <summary>
    /// Get all coupons.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of all coupons.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<CouponDto>), 200)]
    public async Task<ActionResult<List<CouponDto>>> GetAll(CancellationToken ct)
    {
        var coupons = await _couponService.GetAllAsync(ct).ConfigureAwait(false);
        return Ok(coupons);
    }

    /// <summary>
    /// Get all coupons for a specific customer.
    /// </summary>
    /// <param name="customerId">Customer identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of coupons associated with the customer.</returns>
    [HttpGet("customer/{customerId:int}")]
    [ProducesResponseType(typeof(List<CouponDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CouponDto>>> GetByCustomer(int customerId, CancellationToken ct)
    {
        var coupons = await _couponService.GetByCustomerAsync(customerId, ct).ConfigureAwait(false);
        return Ok(coupons);
    }

    /// <summary>
    /// Get all coupons for a specific offer.
    /// </summary>
    /// <param name="offerId">Offer identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of coupons associated with the offer.</returns>
    [HttpGet("offer/{offerId:int}")]
    [ProducesResponseType(typeof(List<CouponDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CouponDto>>> GetByOffer(int offerId, CancellationToken ct)
    {
        var coupons = await _couponService.GetByOfferAsync(offerId, ct).ConfigureAwait(false);
        return Ok(coupons);
    }

    /// <summary>
    /// Get all coupons for a specific merchant.
    /// </summary>
    /// <param name="merchantId">Merchant identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of coupons associated with the merchant.</returns>
    [HttpGet("merchant/{merchantId:int}")]
    [ProducesResponseType(typeof(List<CouponDto>), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(200, typeof(CouponDtoExample))]
    public async Task<IActionResult> GetCouponsByMerchant(int merchantId, CancellationToken ct)
    {
        var coupons = await _couponService.GetCouponsByMerchantAsync(merchantId, ct).ConfigureAwait(false);
        return Ok(coupons);
    }

    /// <summary>
    /// Create a new coupon.
    /// </summary>
    /// <param name="dto">Coupon details.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created coupon.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CouponDto), StatusCodes.Status201Created)]
    [SwaggerRequestExample(typeof(CreateCouponDto), typeof(CreateCouponDtoExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CouponDtoExample))]
    public async Task<ActionResult<CouponDto>> Create([FromBody] CreateCouponDto dto, CancellationToken ct)
    {
        var created = await _couponService.CreateCouponAsync(dto, ct).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Update an existing coupon.
    /// </summary>
    /// <param name="dto">Updated coupon details.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if update is successful.</returns>
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [SwaggerRequestExample(typeof(UpdateCouponDto), typeof(UpdateCouponDtoExample))]
    public async Task<IActionResult> Update([FromBody] UpdateCouponDto dto, CancellationToken ct)
    {
        await _couponService.UpdateCouponAsync(dto, ct).ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Delete a coupon by identifier.
    /// </summary>
    /// <param name="id">Coupon identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if deletion is successful.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _couponService.DeleteCouponAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }
}
