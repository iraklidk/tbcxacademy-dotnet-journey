using Asp.Versioning;
using LibraryManagement.Application.DTOs;
using LibraryManagementAPI.DTOs.BorrowRecords;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    /// <summary>
    /// Controller for managing borrow records.
    /// </summary>
    [ApiController]
    // [Authorize(Roles = "Admin")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/borrow-records")]
    public class BorrowRecordController : ControllerBase
    {
        private readonly IBorrowRecordService _borrowRecordService;

        public BorrowRecordController(IBorrowRecordService borrowRecordService)
        {
            _borrowRecordService = borrowRecordService;
        }

        /// <summary>
        /// Get all borrow records with optional filtering and pagination.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <param name="page">Page number (default 1).</param>
        /// <param name="pageSize">Page size (default 10).</param>
        /// <param name="patronId">Filter by patron ID (optional).</param>
        /// <param name="bookId">Filter by book ID (optional).</param>
        /// <param name="status">Filter by borrow status (optional).</param>
        /// <returns>List of borrow records.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        public async Task<IActionResult> GetAll(CancellationToken ct, int page = 1, int pageSize = 10, int? patronId = null, int? bookId = null,
                                                BorrowStatus? status = null)
        {
            var result = await _borrowRecordService.GetAllAsync(ct, page, pageSize, patronId, bookId, status);
            return Ok(result);
        }

        /// <summary>
        /// Get a borrow record by ID.
        /// </summary>
        /// <param name="id">Borrow record ID.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The borrow record details.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _borrowRecordService.GetByIdAsync(id, ct);
            return Ok(result);
        }

        /// <summary>
        /// Create a new borrow record.
        /// </summary>
        /// <param name="requestModel">Borrow record creation data.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created borrow record.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create(CreateBorrowRecordRequest requestModel, CancellationToken ct)
        {
            var dto = requestModel.Adapt<CreateBorrowRecordDto>();
            var result = await _borrowRecordService.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Mark a borrowed book as returned.
        /// </summary>
        /// <param name="id">Borrow record ID.</param>
        /// <param name="requestModel">Return details.</param>
        /// <param name="ct">Cancellation token.</param>
        [HttpPut("{id:int}/return")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReturnBook(int id, ReturnBorrowRecordRequest requestModel, CancellationToken ct)
        {
            var dto = requestModel.Adapt<ReturnBorrowRecordDto>();
            await _borrowRecordService.ReturnAsync(id, dto, ct);
            return NoContent();
        }

        /// <summary>
        /// Get all overdue borrow records.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of overdue borrow records.</returns>
        [HttpGet("overdue")]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        public async Task<IActionResult> GetOverdue(CancellationToken ct)
        {
            var result = await _borrowRecordService.GetOverdueAsync(ct);
            return Ok(result);
        }
    }
}
