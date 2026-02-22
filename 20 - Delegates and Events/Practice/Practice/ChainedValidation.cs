using System.Text.RegularExpressions;
namespace Practice
{
    public class Chained
    {
        public enum BookGenre
        {
            Fiction,
            Fantasy,
            ScienceFiction,
            Romance,
            History,
            Adventure,
            Drama,
        }
        public class Book
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string ISBN { get; set; }
            public string Publisher { get; set; }
            public DateTime? PublicationDate { get; set; }
            public BookGenre Genre { get; set; }
            public short NumberOfPages { get; set; }
            public bool IsAvailable { get; set; }
            public decimal? Price { get; set; }
        }

        public static class BookValidators
        {
            private static bool OnlyLetters(string input)
            {
                if (string.IsNullOrWhiteSpace(input)) return false;
                foreach (char c in input) if (!char.IsLetter(c) && c != ' ') return false;
                return true;
            }


            public static bool ValidateTitle(Book book, out string field)
            {
                field = "Title";
                return book.Title != null
                       && OnlyLetters(book.Title)
                       && book.Title.Length > 1
                       && book.Title.Length < 255;
            }

            public static bool ValidateAuthor(Book book, out string field)
            {
                field = "Author";
                return book.Author != null
                       && OnlyLetters(book.Author)
                       && book.Author.Length > 3
                       && book.Author.Length < 128;
            }

            public static bool ValidateISBN(Book book, out string field)
            {
                field = book.ISBN;

                if (string.IsNullOrEmpty(book.ISBN))
                    return false;

                if (book.ISBN.Length != 13)
                    return false;

                foreach (char c in book.ISBN)
                {
                    if (!char.IsDigit(c))
                        return false;
                }

                return true;
            }


            public static bool ValidatePublisher(Book book, out string field)
            {
                field = "Publisher";
                return book.Publisher != null
                       && book.Publisher.Length > 2
                       && book.Publisher.Length < 64;
            }

            public static bool ValidatePublicationDate(Book book, out string field)
            {
                field = "PublicationDate";
                return book.PublicationDate is null || book.PublicationDate.Value < DateTime.Now;
            }

            public static bool ValidateGenre(Book book, out string field)
            {
                field = "Genre";

                return Enum.IsDefined(typeof(BookGenre), book.Genre);
            }

            public static bool ValidateNumberOfPages(Book book, out string field)
            {
                field = "NumberOfPages";
                return book.NumberOfPages > 0;
            }

            public static bool ValidateIsAvailable(Book book, out string field)
            {
                field = "IsAvailable";
                return true;
            }

            public static bool ValidatePrice(Book book, out string field)
            {
                field = "Price";
                return book.Price is null || book.Price > 0;
            }
        }

        public delegate bool Validator(Book book, out string field);

        static void Main3()
        {
            var book = new Book
            {
                Title = "1984",
                Author = "George Orwell",
                ISBN = "123",
                Publisher = "P",
                PublicationDate = DateTime.Now.AddDays(1), // future (invalid)
                Genre = BookGenre.Fiction,
                NumberOfPages = 0,
                IsAvailable = true,
                Price = -5
            };

            // declare a multicast delegate
            Validator delegateChain = null;

            // add multiple delegateChain
            delegateChain += BookValidators.ValidateTitle;
            delegateChain += BookValidators.ValidateAuthor;
            delegateChain += BookValidators.ValidateISBN;
            delegateChain += BookValidators.ValidateNumberOfPages;
            delegateChain += BookValidators.ValidatePrice;
            delegateChain += BookValidators.ValidatePublisher;
            delegateChain += BookValidators.ValidateGenre;
            delegateChain += BookValidators.ValidateIsAvailable;
            delegateChain += BookValidators.ValidatePublicationDate;

            // collect ifields
            List<string> invalidFields = new List<string>();

            foreach (Validator v in delegateChain.GetInvocationList())
            {
                if (!v(book, out string field)) invalidFields.Add(field);
            }

            // show results
            if (invalidFields.Count == 0)
                Console.WriteLine("Book is valid ✅");
            else
            {
                Console.WriteLine("Invalid fields:");
                foreach (var f in invalidFields)
                    Console.WriteLine($" - {f}");
            }
        }
    }
}