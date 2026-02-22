using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Exceptions;
using LibraryManagement.Application.RepoInterfaces;
using Mapster;

namespace LibraryManagement.Application.Services.Implementations
{
    public class BorrowRecordService : IBorrowRecordService
    {
        private readonly IBorrowRecordRepository _borrowRecordRepository;
        private readonly IPatronRepository _patronRepository;
        private readonly IBookRepository _bookRepository;

        public BorrowRecordService(IBorrowRecordRepository borrowRecordRepository, IBookRepository bookRepository, IPatronRepository patronRepository)
        {
            _borrowRecordRepository = borrowRecordRepository;
            _bookRepository = bookRepository;
            _patronRepository = patronRepository;
        }

        public async Task<IEnumerable<BorrowRecordDto>> GetAllAsync(CancellationToken ct, int page, int pageSize, int? patronId, int? bookId,
                                                                    BorrowStatus? status)
        {
            var records = await _borrowRecordRepository
                .GetAllAsync(ct, page, pageSize, patronId, bookId, status);
            return records.Adapt<IEnumerable<BorrowRecordDto>>();
        }

        public async Task<BorrowRecordDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var record = await _borrowRecordRepository.GetByIdAsync(id, ct);
            if (record == null)
                throw new NotFoundException("Borrow record with the specified ID does not exist.");
            return record.Adapt<BorrowRecordDto>();
        }

        public async Task<BorrowRecordDto> CreateAsync(CreateBorrowRecordDto dto, CancellationToken ct)
        {
            if (dto.DueDate <= dto.BorrowDate)
                throw new InvalidOperationException("Due date must be after borrow date.");

            using var transaction = await _borrowRecordRepository.BeginTransactionAsync(ct);

            try
            {
                var book = await _bookRepository.GetByIdAsync(dto.BookId, ct);
                if (book == null)
                    throw new NotFoundException("Book with the specified ID does not exist.");

                if (book.Quantity <= 0)
                    throw new InvalidOperationException("This book is currently unavailable.");

                var patron = await _patronRepository.GetByIdAsync(dto.PatronId, ct);
                if (patron == null)
                    throw new NotFoundException("Patron with the specified ID does not exist.");

                book.Quantity -= 1;
                await _bookRepository.UpdateAsync(book, ct);

                var record = dto.Adapt<BorrowRecord>();
                record.Status = BorrowStatus.Borrowed;

                await _borrowRecordRepository.AddAsync(record, ct);
                await transaction.CommitAsync(ct);
                return record.Adapt<BorrowRecordDto>();
            }

            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task ReturnAsync(int id, ReturnBorrowRecordDto dto, CancellationToken ct)
        {
            using var transaction = await _borrowRecordRepository.BeginTransactionAsync(ct);

            try
            {
                var record = await _borrowRecordRepository.GetByIdAsync(id, ct);
                if (record == null)
                    throw new NotFoundException("Borrow record with the specified ID does not exist.");

                if (record.ReturnDate != null)
                    throw new InvalidOperationException("Book has already been returned.");

                record.ReturnDate = dto.ReturnDate;
                record.Status = dto.ReturnDate > record.DueDate ? BorrowStatus.Overdue : BorrowStatus.Returned;

                var book = await _bookRepository.GetByIdAsync(record.BookId, ct)
                           ?? throw new InvalidOperationException($"Book with ID {record.BookId} not found.");

                book.Quantity += 1;

                await _bookRepository.UpdateAsync(book, ct);

                await _borrowRecordRepository.UpdateAsync(record, ct);
                await transaction.CommitAsync(ct);
            }

            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        public async Task<IEnumerable<BorrowRecordDto>> GetOverdueAsync(CancellationToken ct)
        {
            var records = await _borrowRecordRepository.GetOverdueAsync(ct);
            return records.Adapt<IEnumerable<BorrowRecordDto>>();
        }
    }
}
