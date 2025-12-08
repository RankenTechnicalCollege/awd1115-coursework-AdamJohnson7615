using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSchedule.Models.DomainModels;

namespace WorkSchedule.Models.Configuration
{
    public class PositionConfig : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> entity)
        {
           entity.HasOne(c => c.Manager)
                 .WithMany(m => m.Positions)
                 .OnDelete(DeleteBehavior.Restrict);

            entity.HasData(
                new Position { PositionId = 1, Title = "Cashier", Number = 1, ManagerId = 1, DayId = 1, MilitaryTime = "0800" },
                new Position { PositionId = 2, Title = "Stock Clerk", Number = 2, ManagerId = 1, DayId = 1, MilitaryTime = "0900" },
                new Position { PositionId = 3, Title = "Customer Service", Number = 3, ManagerId = 1, DayId = 2, MilitaryTime = "1000" },
                new Position { PositionId = 4, Title = "Cashier", Number = 4, ManagerId = 1, DayId = 2, MilitaryTime = "1100" },
                new Position { PositionId = 5, Title = "Stock Clerk", Number = 5, ManagerId = 1, DayId = 3, MilitaryTime = "1200" },
                new Position { PositionId = 6, Title = "Customer Service", Number = 6, ManagerId = 1, DayId = 3, MilitaryTime = "1300" },
                new Position { PositionId = 7, Title = "Cashier", Number = 7, ManagerId = 1, DayId = 4, MilitaryTime = "1400" },
                new Position { PositionId = 8, Title = "Stock Clerk", Number = 8, ManagerId = 1, DayId = 4, MilitaryTime = "1500" }
                );
        }
    }
}
