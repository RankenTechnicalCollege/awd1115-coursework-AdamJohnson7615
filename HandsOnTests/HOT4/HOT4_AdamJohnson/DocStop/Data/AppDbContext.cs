using Microsoft.EntityFrameworkCore;
using DocStop.Models;
using System;

namespace DocStop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Username = "AdamJohnson",
                    Phone = "555-111-2222"
                },
                new Customer
                {
                    Id = 2,
                    Username = "NicoleHernandez",
                    Phone = "555-333-4444"
                },
                new Customer
                {
                    Id = 3,
                    Username = "CollinMoore",
                    Phone = "555-555-6666"
                }
            );

            // Seed Appointments — must match existing CustomerIds
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    Id = 1,
                    Start = new DateTime(2025, 11, 3, 9, 0, 0),
                    CustomerId = 1
                },
                new Appointment
                {
                    Id = 2,
                    Start = new DateTime(2025, 11, 3, 10, 0, 0),
                    CustomerId = 2
                },
                new Appointment
                {
                    Id = 3,
                    Start = new DateTime(2025, 11, 4, 8, 0, 0),
                    CustomerId = 3
                }
            );
        }
    }
}
