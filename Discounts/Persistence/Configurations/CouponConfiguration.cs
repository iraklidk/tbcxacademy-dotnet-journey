using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Constants;
using Domain.Entities;

namespace Discounts.Infrastructure.Persistence.Configurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(c => c.PurchasedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.Status)
               .HasDefaultValue(CouponStatus.Active);

        builder.Property(c => c.ExpirationDate)
               .IsRequired();

        builder.HasOne(c => c.Customer)
               .WithMany(u => u.Coupons)
               .HasForeignKey(c => c.CustomerId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Offer)
           .WithMany(o => o.Coupons)
           .HasForeignKey(c => c.OfferId)
           .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(c => c.Code)
               .IsUnique();
    }
}
