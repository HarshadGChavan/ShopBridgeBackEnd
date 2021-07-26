using Microsoft.EntityFrameworkCore;
using ShopBridgeBackEnd.Entities;

namespace ShopBridgeBackEnd.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        
        public DbSet<Inventory> Inventories { get; set; }

    }
}
