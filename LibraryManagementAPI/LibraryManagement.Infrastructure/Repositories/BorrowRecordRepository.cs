using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class BorrowRecordRepository : BaseRepository<BorrowRecord>, IBorrowRecordRepository
    {
        public BorrowRecordRepository(LibraryDbContext context) : base(context) { }

        public async Task<IEnumerable<BorrowRecord>> GetAllAsync(CancellationToken ct, int page, int pageSize, int? patronId, int? bookId,
                                                                 BorrowStatus? status)
        {
            IQueryable<BorrowRecord> query = _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Patron);

            if (patronId.HasValue)
                query = query.Where(br => br.PatronId == patronId);

            if (bookId.HasValue)
                query = query.Where(br => br.BookId == bookId);

            if (status.HasValue)
                query = query.Where(br => br.Status == status);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<BorrowRecord>> GetOverdueAsync(CancellationToken ct)
        {
            return await _context.BorrowRecords
                .Where(br =>
                    br.ReturnDate == null &&
                    br.DueDate < DateTime.UtcNow)
                .Include(br => br.Book)
                .Include(br => br.Patron)
                .ToListAsync(ct);
        }
    }
}
