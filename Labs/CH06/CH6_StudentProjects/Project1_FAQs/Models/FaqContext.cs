using Microsoft.EntityFrameworkCore;

namespace Project1_FAQs.Models
{
    public class FaqContext : DbContext
    {
        public FaqContext(DbContextOptions<FaqContext> options) : base(options) { }

        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Topics
            modelBuilder.Entity<Topic>().HasData(
                new Topic { Id = "bootstrap", Name = "Bootstrap" },
                new Topic { Id = "csharp", Name = "C#" },
                new Topic { Id = "javascript", Name = "JavaScript" }
            );

            // Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = "general", Name = "General" },
                new Category { Id = "history", Name = "History" }
            );

            // FAQs
            modelBuilder.Entity<Faq>().HasData(
                // Bootstrap
                new Faq { Id = 1, Question = "What is Bootstrap?", Answer = "Bootstrap is a front-end framework for building responsive web sites and applications.", TopicId = "bootstrap", CategoryId = "general" },
                new Faq { Id = 2, Question = "When was Bootstrap first released?", Answer = "Bootstrap was first released in 2011.", TopicId = "bootstrap", CategoryId = "history" },

                // C#
                new Faq { Id = 3, Question = "What is C#?", Answer = "C# is a modern, object-oriented programming language developed by Microsoft.", TopicId = "csharp", CategoryId = "general" },
                new Faq { Id = 4, Question = "When was C# first released?", Answer = "C# was first released in 2002.", TopicId = "csharp", CategoryId = "history" },

                // JavaScript
                new Faq { Id = 5, Question = "What is JavaScript?", Answer = "JavaScript is a high-level, dynamic programming language used primarily for web development.", TopicId = "javascript", CategoryId = "general" },
                new Faq { Id = 6, Question = "When was JavaScript first released?", Answer = "JavaScript was first released in 1995.", TopicId = "javascript", CategoryId = "history" }
            );
        }
    }
}
