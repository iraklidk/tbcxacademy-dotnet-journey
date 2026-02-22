using Domain.Entities;
using Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Discounts.Infrastructure.Persistence.Configurations;

public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
{
    public void Configure(EntityTypeBuilder<Merchant> builder)
    {
        builder.ToTable("Merchants");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasMany(m => m.Offers)
               .WithOne(o => o.Merchant)
               .HasForeignKey(o => o.MerchantId);

        builder.HasIndex(m => m.UserId)
               .IsUnique();

        builder.Property(c => c.Balance)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.HasIndex(x => x.UserId).IsUnique();

        builder.HasOne<User>()
               .WithOne()
               .HasForeignKey<Merchant>(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
