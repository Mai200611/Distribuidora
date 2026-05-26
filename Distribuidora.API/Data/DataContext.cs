using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Distribuidora.Shared.Entities;

namespace Distribuidora.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Zona> Zonas { get; set; }

        public DbSet<Vehiculo> Vehiculos { get; set; }

        public DbSet<Empleado> Empleados { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Tienda> Tiendas { get; set; }

        public DbSet<RegistroJornada> RegistrosJornada { get; set; }

        public DbSet<DetalleVenta> DetallesVenta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // REGISTRO JORNADA -> VEHICULO
            modelBuilder.Entity<RegistroJornada>()
                .HasOne(r => r.Vehiculo)
                .WithMany()
                .HasForeignKey(r => r.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            // REGISTRO JORNADA -> EMPLEADO
            modelBuilder.Entity<RegistroJornada>()
                .HasOne(r => r.Empleado)
                .WithMany(e => e.RegistrosJornada)
                .HasForeignKey(r => r.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);

            // DECIMALES

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Vehiculo>()
                .Property(v => v.KilometrajeActual)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RegistroJornada>()
                .Property(r => r.KilometrosRecorridos)
                .HasPrecision(18, 2);

            modelBuilder.Entity<RegistroJornada>()
                .Property(r => r.VentaTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleVenta>()
                .Property(d => d.Subtotal)
                .HasPrecision(18, 2);
        }
    }
}