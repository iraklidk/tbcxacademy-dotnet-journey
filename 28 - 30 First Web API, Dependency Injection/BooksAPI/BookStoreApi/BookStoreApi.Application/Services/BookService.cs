using BookStoreApi.Application.Models;

namespace BookStoreApi.Application.Services
{
    public class BookService : IBookService
    {
        public List<Book> _books;
        public Guid guid { get; set; }

        public BookService()
        {
            this.guid = Guid.NewGuid();
            _books = new List<Book>
            {
                new Book { Id = 1, Title = "The Hobbit", Author = "J.R.R. Tolkien" },
                new Book { Id = 2, Title = "1984", Author = "George Orwell" },
                new Book { Id = 3, Title = "Clean Code", Author = "Robert C. Martin" },
                new Book { Id = 4, Title = "To Kill a Mockingbird", Author = "Harper Lee" }
            };
        }

        public Book? GetBook(int id)
        {
            return _books.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Book> GetBooks()
        {
            return _books;
        }
    }
}