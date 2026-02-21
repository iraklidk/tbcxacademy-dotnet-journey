using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Discounts.Infrastructure.Persistence.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.ReservedAt)
               .IsRequired();

        builder.Property(r => r.ExpiresAt)
               .IsRequired();

        builder.HasOne(r => r.Offer)
               .WithMany(o => o.Reservations)
               .HasForeignKey(r => r.OfferId);

        builder.HasOne(r => r.Customer)
               .WithMany(u => u.Reservations)
               .HasForeignKey(r => r.CustomerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
