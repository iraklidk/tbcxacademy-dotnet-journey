using LibraryManagement.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;
namespace LibraryManagement.Persistence.Context
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Patron> Patrons { get; set; } = null!;
        public DbSet<BorrowRecord> BorrowRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                var entities = ChangeTracker
                    .Entries()
                    .Where(e => e.Entity != null &&
                                (e.State == EntityState.Modified ||
                                 e.State == EntityState.Added ||
                                 e.State == EntityState.Deleted))
                    .ToList();

                foreach (var entity in entities)
                {
                    entity.State = entity.State switch
                    {
                        EntityState.Added => EntityState.Modified,
                        _ => EntityState.Unchanged
                    };
                }

                throw;
            }
        }
    }
}