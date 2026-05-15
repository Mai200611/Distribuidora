using Microsoft.EntityFrameworkCore;
using Distribuidora.Shared.Entities;

namespace Distribuidora.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Zona> Zonas { get; set; }
    }
}
