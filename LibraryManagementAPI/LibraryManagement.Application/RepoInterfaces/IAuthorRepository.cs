namespace LibraryManagement.Application.RepoInterfaces
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<IEnumerable<Author>> GetAllAsync(CancellationToken ct, int page, int pageSize);
        Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId, CancellationToken ct);
        Task<Author?> GetByNameAsync(string firstName, string lastName, CancellationToken ct);
    }
}