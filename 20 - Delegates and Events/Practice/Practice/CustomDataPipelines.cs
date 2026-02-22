using System.Collections.Generic;
using static Practice.Chained;

public class BookDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public BookGenre Genre { get; set; }
    public bool IsAvailable { get; set; }
    public decimal? Price { get; set; }
}

namespace Practice
{
    internal class CustomDataPipelines
    {
        static void Main4()
        {
            List<Book> books = new List<Book>
        {
            new Book
            {
                Title = "1984",
                Author = "George Orwell",
                ISBN = "1234567890123",
                Publisher = "Secker & Warburg",
                PublicationDate = new DateTime(1949, 6, 8),
                Genre = BookGenre.Fiction,
                NumberOfPages = 328,
                IsAvailable = true,
                Price = 15.99m
            },
            new Book
            {
                Title = "Brave New World",
                Author = "Aldous Huxley",
                ISBN = "2345678901234",
                Publisher = "Chatto & Windus",
                PublicationDate = new DateTime(1932, 8, 1),
                Genre = BookGenre.ScienceFiction,
                NumberOfPages = 288,
                IsAvailable = false,
                Price = 12.5m
            },
            new Book
            {
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                ISBN = "3456789012345",
                Publisher = "George Allen & Unwin",
                PublicationDate = new DateTime(1937, 9, 21),
                Genre = BookGenre.Fantasy,
                NumberOfPages = 310,
                IsAvailable = true,
                Price = 20.0m
            },
            new Book
            {
                Title = "Clean Code",
                Author = "Robert Martin",
                ISBN = "4567890123456",
                Publisher = "Prentice Hall",
                PublicationDate = new DateTime(2008, 8, 1),
                Genre = BookGenre.ScienceFiction,
                NumberOfPages = 464,
                IsAvailable = true,
                Price = 35.0m
            },
            new Book
            {
                Title = "To Kill a Mockingbird",
                Author = "Harper Lee",
                ISBN = "5678901234567",
                Publisher = "J.B. Lippincott & Co.",
                PublicationDate = new DateTime(1960, 7, 11),
                Genre = BookGenre.Fiction,
                NumberOfPages = 281,
                IsAvailable = false,
                Price = 18.0m
            }
        };

            //// filter books published after 1950
            //List<Book> booksAfter1950 = books
            //    .Where(b => b.PublicationDate.HasValue && b.PublicationDate.Value.Year > 1950)
            //    .ToList();

            //// filter books with less than 400 pages
            //List<Book> shortBooks = books
            //    .Where(b => b.NumberOfPages < 400)
            //    .ToList();

            //// filter books of a specific genre
            //List<Book> fictionBooks = books
            //    .Where(b => b.Genre == BookGenre.Fiction)
            //    .ToList();

            var pipeline = new DataPipeline<Book>()
                .AddFilter(b => b.PublicationDate is not null && b.PublicationDate.Value.Year > 1950)
                .AddFilter(b => b.NumberOfPages < 400)
                .AddTransform(b => new BookDto
                {
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    IsAvailable = b.IsAvailable,
                    Price = b.Price
                });

            var result = pipeline.Process(books);

            foreach (BookDto dto in result)
                Console.WriteLine($"{dto.Title} by {dto.Author}, Genre: {dto.Genre}, Available: {dto.IsAvailable}, Price: {dto.Price}");
        }
    }
}