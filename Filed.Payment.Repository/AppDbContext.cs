using Microsoft.EntityFrameworkCore;
using Filed.Payments.Data.Entities;

namespace Filed.Payments.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Data.Entities.Payment> Payments { get; set; }
        public DbSet<PaymentState> PaymentStates { get; set; }
    }
}
