using MyCollections;

public class CustomSort
{
    public enum BookGenre
    {
        Fantasy,
        ScienceFiction,
        Romance,
        Horror,
        HistoricalFiction,
        Adventure,
        Dystopian,
        History,
        Science,
        Philosophy,

    }
    internal class Book
    {

        public string AuthorFullname { get; set; }
        public string Name { get; set; }
        public short ReleaseDate { get; set; }
        public string ISBN { get; set; }

        public BookGenre Genre { get; set; }
        public Book(string author, string name, short year, string isbn, BookGenre genre)
        => (AuthorFullname, Name, ReleaseDate, ISBN, Genre) = (author, name, year, isbn, genre);
    }


    static void Main()
    {
        List<Book> books = new List<Book>
        {
            new Book("J.K. Rowling", "Harry Potter and the Philosopher's Stone", 1997, "222", BookGenre.Fantasy),
            new Book("George Orwell", "1984", 1949, "111111", BookGenre.Dystopian),
            new Book("William Shakespeare", "Romeo and Juliet", 1813, "999", BookGenre.HistoricalFiction)
        };

        books.Sort(new DateComparer());
        Console.WriteLine("Sort by Release Date:");
        foreach(Book book in books) Console.WriteLine("== "+ book.Name + " == Release Date: " + book.ReleaseDate + " ");

        Console.WriteLine("\nSort by ISBN:");
        IComparer<Book> comparer = new ISBNComparer();
        books.Sort(comparer);
        foreach (Book book in books) Console.WriteLine("== " + book.Name + " == ISBN: " + book.ISBN + " ");
    }
}