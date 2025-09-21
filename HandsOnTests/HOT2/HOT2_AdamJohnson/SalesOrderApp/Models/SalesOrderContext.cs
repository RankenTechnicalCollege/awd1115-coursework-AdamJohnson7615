using Microsoft.EntityFrameworkCore;

namespace SalesOrderApp.Models
{
    public class SalesOrderContext : DbContext
    {
        public SalesOrderContext(DbContextOptions<SalesOrderContext> options)
            : base(options) { }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Systems" },
                new Category { CategoryID = 2, CategoryName = "Games" },
                new Category { CategoryID = 3, CategoryName = "Controllers" },
                new Category { CategoryID = 4, CategoryName = "Protective Gear" },
                new Category { CategoryID = 5, CategoryName = "Audio Equipment" },
                new Category { CategoryID = 6, CategoryName = "Misc Accessories" }
            );

            // Seed products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductID = 1,
                    ProductName = "Nintendo Switch 2",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "",
                    ProductPrice = 450,
                    ProductQty = 15,
                    CategoryID = 1
                },
                new Product
                {
                    ProductID = 2,
                    ProductName = "Mario Kart World",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "",
                    ProductPrice = 80,
                    ProductQty = 45,
                    CategoryID = 2
                },
                new Product
                {
                    ProductID = 3,
                    ProductName = "Joy-Con 2 Pair",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "",
                    ProductPrice = 100,
                    ProductQty = 20,
                    CategoryID = 3
                },
                new Product
                {
                    ProductID = 4,
                    ProductName = "Nintendo Switch 2 Rough Carrying Case",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "",
                    ProductPrice = 30,
                    ProductQty = 7,
                    CategoryID = 4
                },
                new Product
                {
                    ProductID = 5,
                    ProductName = "PanaSonic Ergofit Earbuds ",
                    ProductDescShort = "",
                    ProductDescLong = "",
                    ProductImage = "",
                    ProductPrice = 12,
                    ProductQty = 11,
                    CategoryID = 5
                }
            );
        }
    }
}
