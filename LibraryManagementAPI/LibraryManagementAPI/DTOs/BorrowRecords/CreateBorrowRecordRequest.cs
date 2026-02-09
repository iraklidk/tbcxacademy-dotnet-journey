namespace LibraryManagementAPI.DTOs.BorrowRecords
{
    public class CreateBorrowRecordRequest
    {
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now;
    }
}
