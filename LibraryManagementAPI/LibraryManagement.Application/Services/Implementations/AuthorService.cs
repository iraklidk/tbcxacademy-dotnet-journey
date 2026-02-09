using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Application.Services.Interfaces;
using Mapster;

namespace LibraryManagement.Application.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAsync(CancellationToken ct, int page, int pageSize)
        {
            var authors = await _authorRepository.GetAllAsync(ct, page, pageSize);
            return authors.Adapt<IEnumerable<AuthorDto>>();
        }

        public async Task<AuthorDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var author = await _authorRepository.GetByIdAsync(id, ct);
            if (author == null)
                throw new NotFoundException("Author with the specified ID does not exist.");
            return author?.Adapt<AuthorDto>();
        }

        public async Task<IEnumerable<BookDto>> GetBooksByAuthorIdAsync(int authorId, CancellationToken ct)
        {
            var author = await _authorRepository.GetByIdAsync(authorId, ct);
            if (author == null)
                throw new NotFoundException("Author with the specified ID does not exist.");

            var books = await _authorRepository.GetBooksByAuthorIdAsync(authorId, ct);
            return books.Adapt<IEnumerable<BookDto>>();
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorDto dto, CancellationToken ct)
        {
            string firstName = dto.FirstName.Trim(), lastName = dto.LastName?.Trim() ?? "";
            var existingAuthor = await _authorRepository.GetByNameAsync(firstName, lastName, ct);
            if (existingAuthor != null)
                throw new InvalidOperationException("An author with this name already exists.");
            var author = dto.Adapt<Author>();
            await _authorRepository.AddAsync(author, ct);
            return author.Adapt<AuthorDto>();
        }

        public async Task UpdateAsync(int id, UpdateAuthorDto dto, CancellationToken ct)
        {
            var author = await _authorRepository.GetByIdAsync(id, ct);
            if (author == null) 
                throw new NotFoundException("Author with the specified ID does not exist.");

            var firstName = dto.FirstName.Trim();
            var lastName = dto.LastName.Trim();
            var existingAuthor = await _authorRepository.GetByNameAsync(firstName, lastName, ct);
            if (existingAuthor != null && existingAuthor.Id != id)
                throw new InvalidOperationException("An author with this name already exists.");

            dto.Adapt(author);
            await _authorRepository.UpdateAsync(author, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var author = await _authorRepository.GetByIdAsync(id, ct);
            if (author == null) throw new NotFoundException("Author with the specified ID does not exist."); ;
            await _authorRepository.DeleteAsync(author, ct);
        }

        public Task<Author?> GetByNameAsync(string firstName, string lastName, CancellationToken ct)
        {
            return _authorRepository.GetByNameAsync(firstName, lastName, ct);
        }
    }
}
