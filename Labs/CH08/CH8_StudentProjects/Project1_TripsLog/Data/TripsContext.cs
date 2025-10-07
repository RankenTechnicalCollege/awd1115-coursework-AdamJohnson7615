using Microsoft.EntityFrameworkCore;
using Project1_TripsLog.Models;

namespace Project1_TripsLog.Data
{
    public class TripsContext : DbContext
    {
        public TripsContext(DbContextOptions<TripsContext> options)
            : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }
    }
}
