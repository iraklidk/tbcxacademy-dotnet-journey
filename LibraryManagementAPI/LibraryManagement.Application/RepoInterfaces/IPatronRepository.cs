namespace LibraryManagement.Application.RepoInterfaces
{
    public interface IPatronRepository : IBaseRepository<Patron>
    {
        Task<IEnumerable<Book>> GetBorrowedBooksAsync(int patronId, CancellationToken ct);
        Task<IEnumerable<Patron>> GetAllAsync(CancellationToken ct, int page, int pageSize);
        Task<Patron?> GetByNameAsync(string firstName, string lastName, CancellationToken ct);
    }
}
