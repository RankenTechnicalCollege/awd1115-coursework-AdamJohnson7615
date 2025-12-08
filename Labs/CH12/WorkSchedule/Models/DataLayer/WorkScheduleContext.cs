using Microsoft.EntityFrameworkCore;
using WorkSchedule.Models.Configuration;
using WorkSchedule.Models.DomainModels;

namespace WorkSchedule.Models.DataLayer
{
    public class WorkScheduleContext : DbContext
    {
        public WorkScheduleContext (DbContextOptions<WorkScheduleContext> options)
            : base(options)
        {
        }
        public DbSet<Day> Days { get; set; } = null!;
        public DbSet<Manager> Managers { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfiguration(new DayConfig());
           modelBuilder.ApplyConfiguration(new ManagerConfig());
           modelBuilder.ApplyConfiguration(new PositionConfig());
        }

    }
}
