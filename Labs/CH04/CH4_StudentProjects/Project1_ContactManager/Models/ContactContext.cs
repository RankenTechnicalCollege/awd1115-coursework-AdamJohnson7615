using Microsoft.EntityFrameworkCore;
using System;

namespace Project1_ContactManager.Models
{
    public class ContactContext : DbContext
    {
        public ContactContext(DbContextOptions<ContactContext> options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Family" },
                new Category { CategoryId = 2, Name = "Friend" },
                new Category { CategoryId = 3, Name = "Work" },
                new Category { CategoryId = 4, Name = "Other" }
            );

            modelBuilder.Entity<Contact>().HasData(
            new Contact
            {
                ContactId = 1,
                FirstName = "Adam",
                LastName = "Johnson",
                Phone = "123-456-789",
                Email = "adam_johnson@insideranken.org",
                CategoryId = 1,
                DateAdded = new DateTime(2025, 03, 02, 10, 0, 0)
            },
            new Contact
            {
                ContactId = 2,
                FirstName = "Nicole",
                LastName = "Hernandez",
                Phone = "987-654-321",
                Email = "nicoleh@gmail.com",
                CategoryId = 2,
                DateAdded = new DateTime(2025, 9, 22, 12, 30, 0)
            }
            );

        }
    }
}