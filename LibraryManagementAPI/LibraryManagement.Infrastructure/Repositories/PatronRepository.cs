using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repositories
{
    public class PatronRepository : BaseRepository<Patron>, IPatronRepository
    {
        public PatronRepository(LibraryDbContext context) : base(context) { }

        public async Task<IEnumerable<Patron>> GetAllAsync(CancellationToken ct, int page, int pageSize)
        {
            return await _context.Patrons
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Book>> GetBorrowedBooksAsync(int patronId, CancellationToken ct)
        {
            return await _context.BorrowRecords
                .Where(br => br.PatronId == patronId)
                .Include(br => br.Book)
                .Select(br => br.Book)
                .ToListAsync(ct);
        }

        public async Task<Patron?> GetByNameAsync(string firstName, string lastName, CancellationToken ct)
        {
            return await _context.Patrons
            .FirstOrDefaultAsync(a =>
                a.FirstName == firstName &&
                a.LastName == lastName, ct);
        }
    }
}
