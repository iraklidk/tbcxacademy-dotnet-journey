using LibraryManagement.Application.DTOs;
using LibraryManagementAPI.DTOs.BorrowRecords;
using Swashbuckle.AspNetCore.Filters;

namespace LibraryManagementAPI.Infrastructure.SwaggerExamples
{
    public class CreateBorrowRecordRequestExample
        : IExamplesProvider<CreateBorrowRecordRequest>
    {
        public CreateBorrowRecordRequest GetExamples()
        {
            return new CreateBorrowRecordRequest
            {
                BookId = 12,
                PatronId = 34,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(14)
            };
        }
    }

    public class ReturnBorrowRecordRequestExample
        : IExamplesProvider<ReturnBorrowRecordDto>
    {
        public ReturnBorrowRecordDto GetExamples()
        {
            return new ReturnBorrowRecordDto
            {
                ReturnDate = DateTime.UtcNow,
            };
        }
    }
}
