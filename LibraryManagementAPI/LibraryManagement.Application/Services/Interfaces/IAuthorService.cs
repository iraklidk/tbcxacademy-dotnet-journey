using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.RepoInterfaces;

namespace LibraryManagement.Application.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAsync(CancellationToken ct, int page, int pageSize);
        Task<AuthorDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(int authorId, CancellationToken ct);
        Task<AuthorDto> CreateAsync(CreateAuthorDto dto, CancellationToken ct);
        Task UpdateAsync(int id, UpdateAuthorDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
        Task<Author?> GetByNameAsync(string firstName, string lastName, CancellationToken ct);

    }
}
