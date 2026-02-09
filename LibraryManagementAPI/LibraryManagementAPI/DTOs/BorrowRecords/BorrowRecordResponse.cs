namespace LibraryManagementAPI.DTOs.BorrowRecords
{
    public class BorrowRecordResponse
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowStatus Status { get; set; }
    }
}
