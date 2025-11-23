using CineReviewP2.Models;
using Microsoft.EntityFrameworkCore;

namespace CineReviewP2.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Midia> Midias { get; set; }
        public DbSet<Filme> Filmes { get; set; }
        public DbSet<Serie> Series { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar herança TPH (Table Per Hierarchy)
            modelBuilder.Entity<Midia>()
                .HasDiscriminator<string>("TipoMidia")
                .HasValue<Filme>("Filme")
                .HasValue<Serie>("Serie");

            // Configurar relacionamentos
            modelBuilder.Entity<Nota>()
                .HasOne(n => n.Usuario)
                .WithMany(u => u.Notas)
                .HasForeignKey(n => n.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Nota>()
                .HasOne(n => n.Midia)
                .WithMany(m => m.Notas)
                .HasForeignKey(n => n.MidiaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorito>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.Favoritos)
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorito>()
                .HasOne(f => f.Midia)
                .WithMany(m => m.Favoritos)
                .HasForeignKey(f => f.MidiaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
