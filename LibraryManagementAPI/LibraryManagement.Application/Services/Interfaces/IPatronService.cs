using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Services.Interfaces
{
    public interface IPatronService
    {
        Task<IEnumerable<PatronDto>> GetAllAsync(CancellationToken ct, int page, int pageSize);
        Task<PatronDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<PatronDto> CreateAsync(CreatePatronDto dto, CancellationToken ct);
        Task UpdateAsync(int id, UpdatePatronDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
        Task<IEnumerable<BookDto>> GetBorrowedBooksAsync(int patronId, CancellationToken ct);
        Task<Patron?> GetByNameAsync(string firstName, string lastName, CancellationToken ct);
    }
}
