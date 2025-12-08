using Microsoft.EntityFrameworkCore;

namespace PhoneInventory
{
    public class PhoneDb : DbContext
    {
        public PhoneDb(DbContextOptions<PhoneDb> options) : base(options)
        {
        }
        public DbSet<Phone> Phones { get; set; } //create phones db table
    }
}
