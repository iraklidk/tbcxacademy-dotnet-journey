using LibraryManagement.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Seeding
{
    public static class LibraryDbExtensions
    {
        public static async Task SeedAsync(this LibraryDbContext context, CancellationToken ct = default)
        {
            // Seed Authors
            if (!await context.Authors.AnyAsync(ct))
            {
                var authors = new List<Author>
                {
                    new Author { FirstName = "George", LastName = "Orwell", Biography = "English novelist", DateOfBirth = new DateTime(1903, 6, 25) },
                    new Author { FirstName = "J.K.", LastName = "Rowling", Biography = "British author", DateOfBirth = new DateTime(1965, 7, 31) },
                    new Author { FirstName = "Agatha", LastName = "Christie", Biography = "Famous mystery writer", DateOfBirth = new DateTime(1890, 9, 15) },
                    new Author { FirstName = "F. Scott", LastName = "Fitzgerald", Biography = "American novelist", DateOfBirth = new DateTime(1896, 9, 24) },
                    new Author { FirstName = "Ernest", LastName = "Hemingway", Biography = "American novelist", DateOfBirth = new DateTime(1899, 7, 21) },
                    new Author { FirstName = "Mark", LastName = "Twain", Biography = "American humorist", DateOfBirth = new DateTime(1835, 11, 30) },
                    new Author { FirstName = "Leo", LastName = "Tolstoy", Biography = "Russian novelist", DateOfBirth = new DateTime(1828, 9, 9) },
                    new Author { FirstName = "Jane", LastName = "Austen", Biography = "English novelist", DateOfBirth = new DateTime(1775, 12, 16) },
                    new Author { FirstName = "Charles", LastName = "Dickens", Biography = "English novelist", DateOfBirth = new DateTime(1812, 2, 7) },
                    new Author { FirstName = "H.G.", LastName = "Wells", Biography = "Science fiction author", DateOfBirth = new DateTime(1866, 9, 21) }
                };

                await context.Authors.AddRangeAsync(authors, ct);
                await context.SaveChangesAsync(ct);
            }

            var authorsList = await context.Authors.ToListAsync(ct);

            // Seed Books
            if (!await context.Books.AnyAsync(ct))
            {
                var books = new List<Book>
                {
                    new Book { Title = "1984", ISBN = "9780451524935", PublicationYear = 1949, Description = "Dystopian novel", Quantity = 5, AuthorId = authorsList.First(a => a.LastName == "Orwell").Id },
                    new Book { Title = "Animal Farm", ISBN = "9780451526342", PublicationYear = 1945, Description = "Political satire", Quantity = 4, AuthorId = authorsList.First(a => a.LastName == "Orwell").Id },
                    new Book { Title = "Harry Potter 1", ISBN = "9780439708180", PublicationYear = 1997, Description = "Fantasy novel", Quantity = 10, AuthorId = authorsList.First(a => a.LastName == "Rowling").Id },
                    new Book { Title = "Harry Potter 2", ISBN = "9780439064873", PublicationYear = 1998, Description = "Fantasy novel", Quantity = 8, AuthorId = authorsList.First(a => a.LastName == "Rowling").Id },
                    new Book { Title = "Harry Potter 3", ISBN = "9780439136365", PublicationYear = 1999, Description = "Fantasy novel", Quantity = 7, AuthorId = authorsList.First(a => a.LastName == "Rowling").Id },
                    new Book { Title = "Harry Potter 4", ISBN = "9780439139595", PublicationYear = 2000, Description = "Fantasy novel", Quantity = 6, AuthorId = authorsList.First(a => a.LastName == "Rowling").Id },
                    new Book { Title = "Murder on the Orient Express", ISBN = "9780062693662", PublicationYear = 1934, Description = "Mystery novel", Quantity = 3, AuthorId = authorsList.First(a => a.LastName == "Christie").Id },
                    new Book { Title = "Death on the Nile", ISBN = "9780062073488", PublicationYear = 1937, Description = "Mystery novel", Quantity = 3, AuthorId = authorsList.First(a => a.LastName == "Christie").Id },
                    new Book { Title = "The Great Gatsby", ISBN = "9780743273565", PublicationYear = 1925, Description = "Classic novel", Quantity = 6, AuthorId = authorsList.First(a => a.LastName == "Fitzgerald").Id },
                    new Book { Title = "Tender Is the Night", ISBN = "9780684801544", PublicationYear = 1934, Description = "Classic novel", Quantity = 5, AuthorId = authorsList.First(a => a.LastName == "Fitzgerald").Id },
                    new Book { Title = "The Old Man and the Sea", ISBN = "9780684801223", PublicationYear = 1952, Description = "Novel", Quantity = 4, AuthorId = authorsList.First(a => a.LastName == "Hemingway").Id },
                    new Book { Title = "Adventures of Huckleberry Finn", ISBN = "9780486280615", PublicationYear = 1884, Description = "Classic novel", Quantity = 5, AuthorId = authorsList.First(a => a.LastName == "Twain").Id },
                    new Book { Title = "War and Peace", ISBN = "9780199232765", PublicationYear = 1869, Description = "Epic novel", Quantity = 6, AuthorId = authorsList.First(a => a.LastName == "Tolstoy").Id },
                    new Book { Title = "Pride and Prejudice", ISBN = "9780141439518", PublicationYear = 1813, Description = "Classic novel", Quantity = 5, AuthorId = authorsList.First(a => a.LastName == "Austen").Id },
                    new Book { Title = "Great Expectations", ISBN = "9780141439563", PublicationYear = 1861, Description = "Classic novel", Quantity = 4, AuthorId = authorsList.First(a => a.LastName == "Dickens").Id },
                    new Book { Title = "The Time Machine", ISBN = "9780451530707", PublicationYear = 1895, Description = "Science fiction", Quantity = 3, AuthorId = authorsList.First(a => a.LastName == "Wells").Id },
                    new Book { Title = "Emma", ISBN = "9780141439587", PublicationYear = 1815, Description = "Classic novel", Quantity = 4, AuthorId = authorsList.First(a => a.LastName == "Austen").Id },
                    new Book { Title = "Sense and Sensibility", ISBN = "9780141439662", PublicationYear = 1811, Description = "Classic novel", Quantity = 4, AuthorId = authorsList.First(a => a.LastName == "Austen").Id },
                    new Book { Title = "Oliver Twist", ISBN = "9780141439747", PublicationYear = 1838, Description = "Classic novel", Quantity = 3, AuthorId = authorsList.First(a => a.LastName == "Dickens").Id },
                    new Book { Title = "A Tale of Two Cities", ISBN = "9780141439600", PublicationYear = 1859, Description = "Classic novel", Quantity = 3, AuthorId = authorsList.First(a => a.LastName == "Dickens").Id },
                    new Book { Title = "The Invisible Man", ISBN = "9780451531674", PublicationYear = 1897, Description = "Science fiction", Quantity = 3, AuthorId = authorsList.First(a => a.LastName == "Wells").Id },
                    new Book { Title = "The Island of Doctor Moreau", ISBN = "9780451530660", PublicationYear = 1896, Description = "Science fiction", Quantity = 3, AuthorId = authorsList.First(a => a.LastName == "Wells").Id },
                    new Book { Title = "Anna Karenina", ISBN = "9780140449174", PublicationYear = 1877, Description = "Classic novel", Quantity = 5, AuthorId = authorsList.First(a => a.LastName == "Tolstoy").Id },
                    new Book { Title = "For Whom the Bell Tolls", ISBN = "9780684803357", PublicationYear = 1940, Description = "Novel", Quantity = 4, AuthorId = authorsList.First(a => a.LastName == "Hemingway").Id },
                    new Book { Title = "The Adventures of Tom Sawyer", ISBN = "9780143039563", PublicationYear = 1876, Description = "Classic novel", Quantity = 5, AuthorId = authorsList.First(a => a.LastName == "Twain").Id },
                    new Book { Title = "David Copperfield", ISBN = "9780140439441", PublicationYear = 1850, Description = "Classic novel", Quantity = 4, AuthorId = authorsList.First(a => a.LastName == "Dickens").Id }
                };

                await context.Books.AddRangeAsync(books, ct);
                await context.SaveChangesAsync(ct);
            }

            var booksList = await context.Books.ToListAsync(ct);

            // Seed Patrons (10 patrons)
            if (!await context.Patrons.AnyAsync(ct))
            {
                var patrons = new List<Patron>
                {
                    new Patron { FirstName = "Alice", LastName = "Johnson", Email = "alice@example.com", MembershipDate = DateTime.UtcNow.AddYears(-1) },
                    new Patron { FirstName = "Bob", LastName = "Smith", Email = "bob@example.com", MembershipDate = DateTime.UtcNow.AddMonths(-6) },
                    new Patron { FirstName = "Charlie", LastName = "Brown", Email = "charlie@example.com", MembershipDate = DateTime.UtcNow.AddMonths(-2) },
                    new Patron { FirstName = "David", LastName = "Williams", Email = "david@example.com", MembershipDate = DateTime.UtcNow.AddYears(-2) },
                    new Patron { FirstName = "Eve", LastName = "Davis", Email = "eve@example.com", MembershipDate = DateTime.UtcNow.AddMonths(-8) },
                    new Patron { FirstName = "Frank", LastName = "Miller", Email = "frank@example.com", MembershipDate = DateTime.UtcNow.AddMonths(-4) },
                    new Patron { FirstName = "Grace", LastName = "Wilson", Email = "grace@example.com", MembershipDate = DateTime.UtcNow.AddYears(-1) },
                    new Patron { FirstName = "Hannah", LastName = "Moore", Email = "hannah@example.com", MembershipDate = DateTime.UtcNow.AddMonths(-3) },
                    new Patron { FirstName = "Ian", LastName = "Taylor", Email = "ian@example.com", MembershipDate = DateTime.UtcNow.AddMonths(-5) },
                    new Patron { FirstName = "Jane", LastName = "Anderson", Email = "jane@example.com", MembershipDate = DateTime.UtcNow.AddMonths(-7) }
                };

                await context.Patrons.AddRangeAsync(patrons, ct);
                await context.SaveChangesAsync(ct);
            }

            var patronsList = await context.Patrons.ToListAsync(ct);

            // Seed BorrowRecords (multiple)
            if (!await context.BorrowRecords.AnyAsync(ct))
            {
                var borrowRecords = new List<BorrowRecord>();
                var rand = new Random();

                foreach (var patron in patronsList)
                {
                    // Each patron borrows 2-5 random books
                    var booksToBorrow = booksList.OrderBy(_ => rand.Next()).Take(rand.Next(2, 6)).ToList();
                    foreach (var book in booksToBorrow)
                    {
                        borrowRecords.Add(new BorrowRecord
                        {
                            BookId = book.Id,
                            PatronId = patron.Id,
                            BorrowDate = DateTime.UtcNow.AddDays(-rand.Next(1, 30)),
                            DueDate = DateTime.UtcNow.AddDays(rand.Next(10, 60)),
                            Status = BorrowStatus.Borrowed
                        });
                    }
                }

                await context.BorrowRecords.AddRangeAsync(borrowRecords, ct);
                await context.SaveChangesAsync(ct);
            }
        }

        public static async Task ResetDbAsync(this LibraryDbContext context, CancellationToken ct = default)
        {
            context.BorrowRecords.RemoveRange(context.BorrowRecords);
            context.Patrons.RemoveRange(context.Patrons);
            context.Books.RemoveRange(context.Books);
            context.Authors.RemoveRange(context.Authors);
            await context.SaveChangesAsync();
        }
    }
}
