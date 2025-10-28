using static CustomSort;

internal class DateComparer : IComparer<Book>
{
    public int Compare(Book x, Book y)
    {
        if (x == null || y == null) return 0;

        return x.ReleaseDate.CompareTo(y.ReleaseDate);
    }
}

internal class ISBNComparer : IComparer<Book>
{
    public int Compare(Book x, Book y)
    {
        if (x == null || y == null) return 0;
        return string.Compare(x.ISBN, y.ISBN, StringComparison.Ordinal);
    }
}