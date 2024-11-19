using Microsoft.EntityFrameworkCore;
using Core.Models;
using System.Collections.Generic;

namespace Infraestrutura.Repositories.Data
{
    public class EficazContext : DbContext
    {
        public EficazContext(DbContextOptions<EficazContext> options) : base(options) { }

        public DbSet<SignUp> SignUps { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Marca> Marcas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Endereco>()
                .HasOne(e => e.SignUp)
                .WithMany(s => s.Enderecos)
                .HasForeignKey(e => e.SignUpId);

            modelBuilder.Entity<Token>()
                .HasOne(t => t.SignUp)
                .WithMany(s => s.Tokens)
                .HasForeignKey(t => t.SignUpId);

            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Marca)
                .WithMany(m => m.Produtos)
                .HasForeignKey(p => p.MarcaId);

            // Chamar o SeedData
            SeedData.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }



    }
}
