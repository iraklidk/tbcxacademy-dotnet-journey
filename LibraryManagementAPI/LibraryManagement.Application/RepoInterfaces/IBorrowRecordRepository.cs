using LibraryManagement.Application.RepoInterfaces;

public interface IBorrowRecordRepository : IBaseRepository<BorrowRecord>
{
    Task<IEnumerable<BorrowRecord>> GetAllAsync(CancellationToken ct, int page, int pageSize, int? patronId, int? bookId, BorrowStatus? status);
    Task<IEnumerable<BorrowRecord>> GetOverdueAsync(CancellationToken ct);
}
