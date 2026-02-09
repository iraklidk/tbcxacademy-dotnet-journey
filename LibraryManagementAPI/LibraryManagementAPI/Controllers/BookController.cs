using Asp.Versioning;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Application.Services.Interfaces;
using LibraryManagementAPI.DTOs.Book;
using Mapster;
using Microsoft.AspNetCore.Mvc;


/// <summary>
/// Controller for managing books in the library.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService, IBookRepository bookRepository)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Get all books with pagination.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <param name="page">Page number (default 1).</param>
    /// <param name="pageSize">Page size (default 10).</param>
    /// <returns>List of books with pagination metadata.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BookResponse>), 200)]
    public async Task<IActionResult> GetAll(CancellationToken ct, int page = 1, int pageSize = 10)
    {
        var result = await _bookService.GetAllAsync(ct, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Get a book by its ID.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The book details.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BookResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var book = await _bookService.GetByIdAsync(id, ct);
        if (book == null) return NotFound();
        return Ok(book);
    }

    /// <summary>
    /// Search for books by title or author.
    /// </summary>
    /// <param name="title">Book title (optional).</param>
    /// <param name="author">Author name (optional).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of books matching search criteria.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<BookResponse>), 200)]
    public async Task<IActionResult> Search([FromQuery] string? title, [FromQuery] string? author, CancellationToken ct)
    {
        var results = await _bookService.SearchAsync(title, author, ct);
        return Ok(results);
    }

    /// <summary>
    /// Create a new book.
    /// </summary>
    /// <param name="requestModel">Book creation request.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created book.</returns>
    [HttpPost]
    // [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(BookResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateBookRequest requestModel, CancellationToken ct)
    {
        var appDto = requestModel.Adapt<CreateBookDto>();
        var book = await _bookService.CreateAsync(appDto, ct);
        var response = book.Adapt<BookResponse>();
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Update an existing book.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <param name="requestModel">Book update request.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpPut("{id:int}")]
    // [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookRequest requestModel, CancellationToken ct)
    {
        var appDto = requestModel.Adapt<UpdateBookDto>();
        await _bookService.UpdateAsync(id, appDto, ct);
        return NoContent();
    }

    /// <summary>
    /// Delete a book by ID.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <param name="ct">Cancellation token.</param>
    [HttpDelete("{id:int}")]
    // [Authorize(Roles = "Admin")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _bookService.DeleteAsync(id, ct);
        return NoContent();
    }

    /// <summary>
    /// Check availability of a book.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Whether the book is available.</returns>
    [HttpGet("{id:int}/availability")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CheckAvailability(int id, CancellationToken ct)
    {
        var available = await _bookService.IsAvailableAsync(id, ct);
        return Ok(new { BookId = id, Available = available });
    }
}
