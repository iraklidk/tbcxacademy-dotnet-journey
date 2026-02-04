using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public class BookRepository : BaseRepository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context) { }

    public async Task<List<Book>> GetAllAsync(CancellationToken ct, int page, int pageSize) =>
        await _context.Books
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<List<Book>> SearchAsync(string? title, string? author, CancellationToken ct)
    {
        var query = _context.Books
            .Include(b => b.Author)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(b => b.Title.Contains(title));

        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(b => (b.Author.FirstName + " " + b.Author.LastName)
                                         .Contains(author));

        return await query.ToListAsync(ct);
    }

    public async Task<bool> IsAvailableAsync(int bookId, CancellationToken ct)
    {
        var book = await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == bookId, ct);

        if (book == null) return false;

        return book.Quantity > 0;
    }

    public async Task<Book?> GetByTitleAsync(string title, CancellationToken ct)
    {
        return await _context.Books
            .FirstOrDefaultAsync(b => b.Title == title, ct);
    }
}
