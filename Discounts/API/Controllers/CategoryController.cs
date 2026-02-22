using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Category;
using Swashbuckle.AspNetCore.Filters;
using Application.Interfaces.Services;
using API.Infrastructure.SwaggerExamples;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

/// <summary>
/// Provides endpoints for managing categories.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService) => _categoryService = categoryService;

    /// <summary>
    /// Get category by identifier.
    /// </summary>
    /// <param name="id">Category identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Category details.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoryDto), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(CategoryDtoExample))]
    public async Task<ActionResult<CategoryDto>> GetById(int id, CancellationToken ct)
    {
        var category = await _categoryService.GetByIdAsync(id, ct).ConfigureAwait(false);
        return Ok(category);
    }

    /// <summary>
    /// Get all categories.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>List of all categories.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), 200)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(CancellationToken ct)
    {
        var categories = await _categoryService.GetAllAsync(ct).ConfigureAwait(false);
        return Ok(categories);
    }

    /// <summary>
    /// Create a new category.
    /// </summary>
    /// <param name="dto">Category creation details.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created category.</returns>
    [HttpPost]
    [ProducesResponseType(400)]
    [ProducesResponseType(typeof(CategoryDto), 201)]
    [SwaggerResponseExample(201, typeof(CategoryDtoExample))]
    [SwaggerRequestExample(typeof(CreateCategoryDto), typeof(CreateCategoryDtoExample))]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto, CancellationToken ct)
    {
        var created = await _categoryService.CreateAsync(dto, ct).ConfigureAwait(false);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing category with the provided details.
    /// </summary>
    /// <param name="dto">An object containing the updated category information.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> to cancel the operation if needed.</param>
    /// <returns>
    /// Returns <see cref="NoContentResult"/> (HTTP 204) if the update succeeds. 
    /// Returns HTTP 400 if the input data is invalid, or HTTP 404 if the category does not exist.
    /// </returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerRequestExample(typeof(UpdateCategoryDto), typeof(UpdateCategoryDtoExample))]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryDto dto, CancellationToken ct)
    {
        await _categoryService.UpdateAsync(dto, ct).ConfigureAwait(false);
        return NoContent();
    }

    /// <summary>
    /// Delete a category by identifier.
    /// </summary>
    /// <param name="id">Category identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content if deletion is successful.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _categoryService.DeleteAsync(id, ct).ConfigureAwait(false);
        return NoContent();
    }
}
