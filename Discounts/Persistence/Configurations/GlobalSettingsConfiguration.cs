using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class GlobalSettingsConfiguration : IEntityTypeConfiguration<GlobalSettings>
{
    public void Configure(EntityTypeBuilder<GlobalSettings> builder)
    {
        builder.ToTable("GlobalSettings");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.BookingDurationMinutes)
            .IsRequired()
            .HasDefaultValue(30);

        builder.Property(g => g.MerchantEditHours)
            .IsRequired()
            .HasDefaultValue(24);

        builder.Property(g => g.ReservationPrice)
            .IsRequired()
            .HasDefaultValue(5);
    }
}
