using Microsoft.EntityFrameworkCore;
using PlushShop.Models;

namespace PlushShop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure Slug is unique
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Slug)
                .IsUnique();
        }
    }
}