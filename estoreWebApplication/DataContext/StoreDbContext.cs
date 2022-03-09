#nullable disable
using estoreWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace estoreWebApplication.DataContext
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext()
        {
        }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
              : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}
