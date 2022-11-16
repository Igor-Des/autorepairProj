using autorepairProj.Models;
using Microsoft.EntityFrameworkCore;

namespace autorepairProj.Data
{
    public class AutorepairContext : DbContext
    {
        public AutorepairContext(DbContextOptions<AutorepairContext> options) : base(options)
        {

        }

        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Payment> Payments { get; set; }
    }
}
