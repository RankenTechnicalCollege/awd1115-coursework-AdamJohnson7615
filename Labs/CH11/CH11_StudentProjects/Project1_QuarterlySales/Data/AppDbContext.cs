using Microsoft.EntityFrameworkCore;
using Project1_QuarterlySales.Models;

namespace Project1_QuarterlySales.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Sales> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed one
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "Adam",
                    LastName = "Johnson",
                    DateOfBirth = new DateTime(2002, 3, 2),
                    DateOfHire = new DateTime(2024, 3, 15),
                    ManagerId = 1
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
