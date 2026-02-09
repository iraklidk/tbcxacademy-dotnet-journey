using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryDbContext context) : base(context) { }

    public async Task<IEnumerable<Author>> GetAllAsync(CancellationToken ct, int page, int pageSize)
    {
        return await _context.Authors
            .OrderBy(a => a.LastName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId, CancellationToken ct)
    {
        return await _context.Books
            .Where(b => b.AuthorId == authorId)
            .ToListAsync(ct);
    }

    public async Task<Author?> GetByNameAsync(string firstName, string lastName, CancellationToken ct)
    {
        return await _context.Authors
        .FirstOrDefaultAsync(a =>
            a.FirstName == firstName &&
            a.LastName == lastName, ct);
    }
}
