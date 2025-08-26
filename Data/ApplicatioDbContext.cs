using Microsoft.EntityFrameworkCore;
using SalesManagementApp.Models;
using TallerNett.Models;

namespace TallerNett.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Producto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<DetalleVenta> Detalle { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AGREGAR: Mapear explícitamente los nombres de tabla según la migración
            modelBuilder.Entity<Cliente>().ToTable("Cliente");
            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<Venta>().ToTable("Venta");
            modelBuilder.Entity<DetalleVenta>().ToTable("Detalle");

            // Configurar precisión de decimales
            modelBuilder.Entity<Producto>()
                .Property(d => d.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Venta>()
                .Property(d => d.Total)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<DetalleVenta>()
                .Property(d => d.Subtotal)
                .HasColumnType("decimal(18,2)");

            // Configurar relaciones
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Venta)
                .WithMany(v => v.DetalleVentas)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}