using BookStoreApi.Application.Models;

namespace BookStoreApi.Application.Services
{
    public interface IBookService
    {
        public Guid guid { get; }
        IEnumerable<Book> GetBooks();
        Book? GetBook(int id);
    }
}