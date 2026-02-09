

namespace LibraryManagement.Application.RepoInterfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<List<Book>> GetAllAsync(CancellationToken ct, int page, int pageSize);
        Task<List<Book>> SearchAsync(string? title, string? author, CancellationToken ct);
        Task<bool> IsAvailableAsync(int id, CancellationToken ct);
        Task<Book?> GetByTitleAsync(string title, CancellationToken ct);
    }
}