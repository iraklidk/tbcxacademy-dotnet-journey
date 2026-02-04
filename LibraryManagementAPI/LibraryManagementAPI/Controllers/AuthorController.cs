using Asp.Versioning;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Services.Interfaces;
using LibraryManagementAPI.DTOs.Author;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Handles all author-related API endpoints.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
// [Authorize(Roles = "Admin")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    /// <summary>
    /// Get all authors with pagination.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <param name="page">Page number (default 1).</param>
    /// <param name="pageSize">Page size (default 10).</param>
    /// <returns>List of authors with pagination metadata.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AuthorResponse>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct, int page = 1, int pageSize = 10)
    {
        var authors = await _authorService.GetAllAsync(ct, page, pageSize);
        return Ok(authors);
    }

    /// <summary>
    /// Get an author by ID.
    /// </summary>
    /// <param name="id">Author ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The author details.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AuthorResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var author = await _authorService.GetByIdAsync(id, ct);
        return Ok(author);
    }


    /// <summary>
    /// Get all books written by an author.
    /// </summary>
    /// <param name="id">Author ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of books by the author.</returns>
    [HttpGet("{id:int}/books")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetBooks(int id, CancellationToken ct)
    {
        var books = await _authorService.GetBooksByAuthorIdAsync(id, ct);
        return Ok(books);
    }

    /// <summary>
    /// Create a new author.
    /// </summary>
    /// <param name="request">Author creation request.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created author.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AuthorResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateAuthorRequest request, CancellationToken ct)
    {
        var dto = request.Adapt<CreateAuthorDto>();
        var author = await _authorService.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    /// <summary>
    /// Update an existing author.
    /// </summary>
    /// <param name="id">Author ID.</param>
    /// <param name="request">Author update request.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAuthorRequest request, CancellationToken ct)
    {
        var dto = request.Adapt<UpdateAuthorDto>();
        await _authorService.UpdateAsync(id, dto, ct);
        return NoContent();
    }

    /// <summary>
    /// Delete an author by ID.
    /// </summary>
    /// <param name="id">Author ID.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _authorService.DeleteAsync(id, ct);
        return NoContent();
    }
}
