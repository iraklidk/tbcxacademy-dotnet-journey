using Asp.Versioning;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Services.Interfaces;
using LibraryManagementAPI.DTOs.Book;
using LibraryManagementAPI.DTOs.Patrons;
using Mapster;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing patrons.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
// [Authorize(Roles = "Admin")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PatronController : ControllerBase
{
    private readonly IPatronService _patronService;

    public PatronController(IPatronService patronService)
    {
        _patronService = patronService;
    }

    /// <summary>
    /// Get all patrons with pagination.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <param name="page">Page number (default 1).</param>
    /// <param name="pageSize">Page size (default 10).</param>
    /// <returns>List of patrons.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PatronResponse>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct, int page = 1, int pageSize = 10)
    {
        var patrons = await _patronService.GetAllAsync(ct,page, pageSize);
        return Ok(patrons);
    }

    /// <summary>
    /// Get a patron by ID.
    /// </summary>
    /// <param name="id">Patron ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The patron details.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PatronResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var patron = await _patronService.GetByIdAsync(id, ct);
        if (patron == null) return NotFound();
        return Ok(patron);
    }

    /// <summary>
    /// Get all books currently borrowed by a patron.
    /// </summary>
    /// <param name="id">Patron ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of borrowed books.</returns>
    [HttpGet("{id:int}/books")]
    [ProducesResponseType(typeof(IEnumerable<BookResponse>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBooks(int id, CancellationToken ct)
    {
        var books = await _patronService.GetBorrowedBooksAsync(id, ct);
        return Ok(books);
    }

    /// <summary>
    /// Create a new patron.
    /// </summary>
    /// <param name="request">Patron creation request.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created patron.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PatronResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreatePatronRequest request, CancellationToken ct)
    {
        var dto = request.Adapt<CreatePatronDto>();
        var patron = await _patronService.CreateAsync(dto, ct);
        var response = patron.Adapt<PatronResponse>();
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Update an existing patron.
    /// </summary>
    /// <param name="id">Patron ID.</param>
    /// <param name="request">Patron update request.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePatronRequest request, CancellationToken ct)
    {
        var dto = request.Adapt<UpdatePatronDto>();
        await _patronService.UpdateAsync(id, dto, ct);
        return NoContent();
    }

    /// <summary>
    /// Delete a patron by ID.
    /// </summary>
    /// <param name="id">Patron ID.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _patronService.DeleteAsync(id, ct);
        return NoContent();
    }
}
