using Microsoft.EntityFrameworkCore;
using Practica_2.Domain.Entidades;

namespace Practica_2.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; } = null!;

        public DbSet<Categoria> Categorias { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasOne(p => p.Categoria)
                    .WithMany(c => c.Productos)
                    .HasForeignKey(p => p.CategoriaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
