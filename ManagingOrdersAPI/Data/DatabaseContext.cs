using OrderManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

       

    }
   
}
