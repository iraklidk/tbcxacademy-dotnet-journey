using Domain.Entities;
using Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Discounts.Persistence.Context;

public class DiscountsDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DiscountsDbContext(DbContextOptions<DiscountsDbContext> options)
        : base(options) { }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Merchant> Merchants { get; set; }

    public DbSet<Offer> Offers { get; set; }

    public DbSet<Reservation> Reservations { get; set; }

    public DbSet<Coupon> Coupons { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<GlobalSettings> GlobalSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscountsDbContext).Assembly);
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole<int>>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
    }
}
