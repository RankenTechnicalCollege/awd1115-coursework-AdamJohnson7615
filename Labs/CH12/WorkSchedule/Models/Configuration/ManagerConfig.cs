using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSchedule.Models.DomainModels;

namespace WorkSchedule.Models.Configuration
{
    public class ManagerConfig : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> entity)
        {
           entity.HasData(
                new Manager { ManagerId = 1, FirstName = "Alice", LastName="Jenkins" },
                new Manager { ManagerId = 2, FirstName = "Bob", LastName="Evans" },
                new Manager { ManagerId = 3, FirstName = "Carol", LastName="Henderson" },
                new Manager { ManagerId = 4, FirstName = "Manuel", LastName="Gutierez"},
                new Manager { ManagerId = 5, FirstName = "Ulysses", LastName="Netherton"});
        }
    }
}
