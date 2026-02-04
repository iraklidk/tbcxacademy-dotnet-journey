using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.RepoInterfaces;
using LibraryManagement.Application.Services.Interfaces;
using Mapster;

namespace LibraryManagement.Application.Services.Implementations
{
    public class PatronService : IPatronService
    {
        private readonly IPatronRepository _patronRepository;

        public PatronService(IPatronRepository patronRepository)
        {
            _patronRepository = patronRepository;
        }

        public async Task<IEnumerable<PatronDto>> GetAllAsync(CancellationToken ct, int page, int pageSize)
        {
            var patrons = await _patronRepository.GetAllAsync(ct, page, pageSize);
            return patrons.Adapt<IEnumerable<PatronDto>>();
        }

        public async Task<PatronDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var patron = await _patronRepository.GetByIdAsync(id, ct);
            if(patron is null) throw new NotFoundException("Patron with the specified ID does not exist.");
            return patron.Adapt<PatronDto>();
        }

        public async Task<PatronDto> CreateAsync(CreatePatronDto dto, CancellationToken ct)
        {
            var firstName = dto.FirstName.Trim();
            var lastName = dto.LastName?.Trim() ?? "";
            var existingPatron = await _patronRepository.GetByNameAsync(firstName, lastName, ct);
            if (existingPatron != null)
                throw new InvalidOperationException("Patron already exists");
            var patron = dto.Adapt<Patron>();
            await _patronRepository.AddAsync(patron, ct);
            return patron.Adapt<PatronDto>();
        }

        public async Task UpdateAsync(int id, UpdatePatronDto dto, CancellationToken ct)
        {
            var patron = await _patronRepository.GetByIdAsync(id, ct);
            if (patron == null)
                throw new NotFoundException("Patron with the specified ID does not exist.");

            var firstName = dto.FirstName.Trim();
            var lastName = dto.LastName?.Trim() ?? "";
            var existingPatron = await _patronRepository.GetByNameAsync(firstName, lastName, ct);
            if (existingPatron != null && existingPatron.Id != id)
                throw new InvalidOperationException("A patron with this name already exists.");

            dto.Adapt(patron);
            await _patronRepository.UpdateAsync(patron, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var patron = await _patronRepository.GetByIdAsync(id, ct);
            if (patron == null)
                throw new NotFoundException("Patron with the specified ID does not exist.");

            await _patronRepository.DeleteAsync(patron, ct);
        }

        public async Task<IEnumerable<BookDto>> GetBorrowedBooksAsync(int patronId, CancellationToken ct)
        {
            var patron = await _patronRepository.GetByIdAsync(patronId, ct);
            if (patron == null)
                throw new NotFoundException("Patron with the specified ID does not exist.");

            var books = await _patronRepository.GetBorrowedBooksAsync(patronId, ct);
            return books.Adapt<IEnumerable<BookDto>>();
        }

        public Task<Patron?> GetByNameAsync(string firstName, string lastName, CancellationToken ct)
        {
            return _patronRepository.GetByNameAsync(firstName, lastName, ct);
        }
    }
}
