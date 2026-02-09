namespace LibraryManagement.Application.DTOs
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowStatus Status { get; set; }
    }

    public class CreateBorrowRecordDto
    {
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class ReturnBorrowRecordDto
    {
        public DateTime ReturnDate { get; set; } = DateTime.Now;
    }
}
