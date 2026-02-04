using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Services.Interfaces
{
    public interface IBookService
    {
        Task<BookDto> CreateAsync(CreateBookDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
        Task<IEnumerable<BookDto>> GetAllAsync(CancellationToken ct, int page, int pageSize);
        Task<BookDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<bool> IsAvailableAsync(int id, CancellationToken ct);
        Task<List<BookDto>> SearchAsync(string? title, string? author, CancellationToken ct);
        Task UpdateAsync(int id, UpdateBookDto dto, CancellationToken ct);
    }
}
