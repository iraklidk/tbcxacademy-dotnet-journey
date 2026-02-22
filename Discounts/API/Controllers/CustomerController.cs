using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Customer;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers;

/// <summary>
/// Provides endpoints for managing customers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomerController(ICustomerService customerService) => _customerService = customerService;

    /// <summary>
    /// Gets a customer by its identifier.
    /// </summary>
    /// <param name="id">Customer identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested customer.</returns>
    /// <response code="200">Returns the customer.</response>
    /// <response code="404">If the customer is not found.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetById(int id, CancellationToken ct)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id, ct).ConfigureAwait(false);
        return Ok(customer);
    }

    /// <summary>
    /// Gets all customers.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of customers.</returns>
    /// <response code="200">Returns all customers.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll(CancellationToken ct)
    {
        var customers = await _customerService.GetAllCustomersAsync(ct).ConfigureAwait(false);
        return Ok(customers);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="customer">Customer data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Created customer.</returns>
    /// <response code="201">Customer created successfully.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDto customer, CancellationToken ct)
    {
        var created = await _customerService.AddCustomerAsync(customer, ct).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, customer);
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="customer">Updated customer data.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if update succeeds.</returns>
    /// <response code="204">Customer updated successfully.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromBody] UpdateCustomerDto customer, CancellationToken ct)
    {
        await _customerService.UpdateCustomerAsync(customer, ct).ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Gets a customer by user identifier.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The requested customer.</returns>
    /// <response code="200">Returns the customer.</response>
    /// <response code="404">If not found.</response>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(CustomerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetByUserId(int userId, CancellationToken ct)
    {
        var customer = await _customerService.GetCustomerByUserIdAsync(userId, ct).ConfigureAwait(false);
        return Ok(customer);
    }

    /// <summary>
    /// Deletes a customer by its identifier.
    /// </summary>
    /// <param name="id">Customer identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if deletion succeeds.</returns>
    /// <response code="204">Customer deleted successfully.</response>
    /// <response code="404">If the customer is not found.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _customerService.DeleteCustomerAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }
}
