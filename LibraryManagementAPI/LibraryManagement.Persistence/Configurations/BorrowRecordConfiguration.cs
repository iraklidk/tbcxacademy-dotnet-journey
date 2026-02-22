using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configurations
{
    public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            builder.ToTable("BorrowRecords");

            builder.HasKey(br => br.Id);

            builder.Property(br => br.BorrowDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(br => br.DueDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(br => br.ReturnDate)
                .HasColumnType("datetime");

            builder.Property(br => br.Status)
                .IsRequired();

            builder.HasOne(br => br.Book)
                .WithMany()
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(br => br.Patron)
                .WithMany(p => p.BorrowRecords)
                .HasForeignKey(br => br.PatronId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
