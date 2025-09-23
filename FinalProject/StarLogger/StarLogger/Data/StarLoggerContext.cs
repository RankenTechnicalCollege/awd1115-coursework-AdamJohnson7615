using Microsoft.EntityFrameworkCore;
using StarLogger.Models;
using System;

namespace StarLogger.Data
{
    public class StarLoggerContext : DbContext
    {
        public StarLoggerContext(DbContextOptions<StarLoggerContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure cascade delete for Achievements when a Game is deleted
            modelBuilder.Entity<Achievement>()
                .HasOne(a => a.Game)
                .WithMany(g => g.Achievements)
                .HasForeignKey(a => a.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial games
            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    GameId = 1,
                    Title = "Minesweeper",
                    Platform = "PC",
                    ReleaseDate = new DateTime(2023, 8, 1),
                    Genre = "Strategy"
                },
                new Game
                {
                    GameId = 2,
                    Title = "Super Mario Odyssey",
                    Platform = "Nintendo Switch",
                    ReleaseDate = new DateTime(2017, 10, 27),
                    Genre = "Platformer"
                }
            );

            // Seed achievements
            modelBuilder.Entity<Achievement>().HasData(
                new Achievement
                {
                    AchievementId = 1,
                    GameId = 1,
                    Name = "First Click",
                    Description = "Click the first square without hitting a mine",
                    Points = 10
                },
                new Achievement
                {
                    AchievementId = 2,
                    GameId = 1,
                    Name = "Win First Game",
                    Description = "Successfully clear your first Minesweeper board",
                    Points = 20
                },
                new Achievement
                {
                    AchievementId = 3,
                    GameId = 2,
                    Name = "Moon Collector",
                    Description = "Collect your first Power Moon",
                    Points = 5
                }
            );
        }
    }
}
