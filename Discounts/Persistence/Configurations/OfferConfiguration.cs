using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Discounts.Persistence.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(o => o.Description)
               .IsRequired()
               .HasMaxLength(2000);

        builder.Property(o => o.OriginalPrice)
               .HasColumnType("decimal(18,2)");

        builder.Property(o => o.DiscountedPrice)
               .HasColumnType("decimal(18,2)");

        builder.Property(o => o.Status)
               .HasConversion<int>();

        builder.HasOne(o => o.Merchant)
               .WithMany(m => m.Offers)
               .HasForeignKey(o => o.MerchantId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
