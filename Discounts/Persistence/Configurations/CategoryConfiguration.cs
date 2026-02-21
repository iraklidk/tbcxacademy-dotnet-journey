using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Discounts.Infrastructure.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasMany(c => c.Offers)
               .WithOne(o => o.Category)
               .HasForeignKey(o => o.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
