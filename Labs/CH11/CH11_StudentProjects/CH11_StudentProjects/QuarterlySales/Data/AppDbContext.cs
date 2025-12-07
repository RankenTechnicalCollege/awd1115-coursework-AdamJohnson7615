using Microsoft.EntityFrameworkCore;
using QuarterlySales.Models;

namespace QuarterlySales.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Sales> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "Adam",
                    LastName = "Johnson",
                    DOB = new DateTime(2002, 03, 02),
                    DateOfHire = new DateTime(2025, 03, 19),
                    ManagerId = 0 // has no manager
                }
            );
        }
    }
}
