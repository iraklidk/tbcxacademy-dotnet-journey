using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Application.Services.Interfaces;
using Mapster;

namespace LibraryManagement.Application.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        public async Task<BookDto> CreateAsync(CreateBookDto dto, CancellationToken ct)
        {
            using var transaction = await _bookRepository.BeginTransactionAsync(ct);

            try
            {
                var names = dto.Author.Trim().Split(' ', 2);
                string firstName = names[0];
                string lastName = names.Length > 1 ? names[1] : "";

                var author = await _authorRepository.GetByNameAsync(firstName, lastName, ct);
                if (author == null)
                {
                    author = new Author
                    {
                        FirstName = firstName,
                        LastName = lastName
                    };
                    await _authorRepository.AddAsync(author, ct);
                }

                var existingBook = await _bookRepository.GetByTitleAsync(dto.Title, ct);
                if (existingBook != null)
                    throw new InvalidOperationException("A book with this title already exists.");

                var book = dto.Adapt<Book>();
                book.AuthorId = author.Id;
                book.Author = author;

                await _bookRepository.AddAsync(book, ct);
                await transaction.CommitAsync(ct);
                return book.Adapt<BookDto>();
            }

            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var book = await _bookRepository.GetByIdAsync(id, ct);
            if (book == null)
                throw new NotFoundException("Book with the specified ID does not exist.");
            await _bookRepository.DeleteAsync(book, ct);
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync(CancellationToken ct, int page, int pageSize)
        {
            var books = await _bookRepository.GetAllAsync(ct, page, pageSize);
            return books.Adapt<IEnumerable<BookDto>>();
        }

        public async Task<BookDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var book = await _bookRepository.GetByIdAsync(id, ct);
            if (book == null)
                throw new NotFoundException("Book with the specified ID does not exist.");
            return book.Adapt<BookDto>();
        }

        public async Task<bool> IsAvailableAsync(int id, CancellationToken ct)
        {
            var book = await _bookRepository.GetByIdAsync(id, ct);
            if (book == null)
                throw new NotFoundException("Book with the specified ID does not exist.");
            return await _bookRepository.IsAvailableAsync(id, ct);
        }

        public async Task<List<BookDto>> SearchAsync(string? title, string? author, CancellationToken ct)
        {
            var books = await _bookRepository.SearchAsync(title, author, ct);
            return books.Adapt<List<BookDto>>();
        }

        public async Task UpdateAsync(int id, UpdateBookDto dto, CancellationToken ct)
        {
            using var transaction = await _bookRepository.BeginTransactionAsync(ct);

            try
            {
                var book = await _bookRepository.GetByIdAsync(id, ct);
                if (book == null)
                    throw new NotFoundException("Book with the specified ID does not exist.");

                var names = dto.Author.Trim().Split(' ', 2);
                string firstName = names[0];
                string lastName = names.Length > 1 ? names[1] : "";

                var author = await _authorRepository.GetByNameAsync(firstName, lastName, ct);
                if (author == null)
                {
                    author = new Author
                    {
                        FirstName = firstName,
                        LastName = lastName
                    };
                    await _authorRepository.AddAsync(author, ct);
                }

                var existingBook = await _bookRepository.GetByTitleAsync(dto.Title.Trim(), ct);
                if (existingBook != null && existingBook.Id != id)
                    throw new InvalidOperationException("A book with this title already exists.");

                book.AuthorId = author.Id;
                dto.Adapt(book);
                await _bookRepository.UpdateAsync(book, ct);
                await transaction.CommitAsync(ct);
            }

            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }
}
