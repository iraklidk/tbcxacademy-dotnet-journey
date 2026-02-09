using LibraryManagement.Application.DTOs;

public interface IBorrowRecordService
{
    Task<IEnumerable<BorrowRecordDto>> GetAllAsync(CancellationToken ct, int page, int pageSize, int? patronId, int? bookId,
                                                    BorrowStatus? status);

    Task<BorrowRecordDto> GetByIdAsync(int id, CancellationToken ct);
    Task<BorrowRecordDto> CreateAsync(CreateBorrowRecordDto dto, CancellationToken ct);
    Task ReturnAsync(int id, ReturnBorrowRecordDto dto, CancellationToken ct);
    Task<IEnumerable<BorrowRecordDto>> GetOverdueAsync(CancellationToken ct);
}
