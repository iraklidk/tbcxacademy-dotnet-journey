using Domain.Entities;
using Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Discounts.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Firstname)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.Lastname)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(c => c.Balance)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.HasMany(c => c.Coupons)
               .WithOne(c => c.Customer)
               .HasForeignKey(c => c.CustomerId)
               .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(c => c.Reservations)
               .WithOne(r => r.Customer)
               .HasForeignKey(r => r.CustomerId);

        builder.HasIndex(x => x.UserId).IsUnique();

        builder.HasOne<User>()
               .WithOne()
               .HasForeignKey<Customer>(u => u.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
