using BookStoreApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IBookService _bookService1;

        public BooksController(IBookService bookService, IBookService bookService1)
        {
            _bookService = bookService;
            _bookService1 = bookService1;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _bookService.GetBooks();

            ConsoleLog();

            return Ok(books);
        }

        private void ConsoleLog()
        {
            Console.WriteLine($"{_bookService.guid}");
            Console.WriteLine($"{_bookService1.guid}");
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _bookService1.GetBook(id);

            if (book == null) return NotFound();

            ConsoleLog();

            return Ok(book);
        }

    }
}