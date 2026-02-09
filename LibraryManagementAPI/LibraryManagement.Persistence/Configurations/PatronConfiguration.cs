using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configurations
{
    public class PatronConfiguration : IEntityTypeConfiguration<Patron>
    {
        public void Configure(EntityTypeBuilder<Patron> builder)
        {
            builder.ToTable("Patrons");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(p => p.MembershipDate)
                .IsRequired()
                .HasColumnType("datetime");

            builder.HasMany(p => p.BorrowRecords)
                .WithOne(br => br.Patron)
                .HasForeignKey(br => br.PatronId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
