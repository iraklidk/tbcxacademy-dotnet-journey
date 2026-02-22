namespace LibraryManagementAPI.DTOs.BorrowRecords
{
    public class ReturnBorrowRecordRequest
    {
        public DateTime ReturnDate { get; set; } = DateTime.Now;
    }
}
