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

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Adicionando uma marca de exemplo
            modelBuilder.Entity<Marca>().HasData(
                new Marca { Id = 1, Nome = "Gabini" }
            );

            // Adicionando produtos de exemplo com o caminho da imagem
            modelBuilder.Entity<Produto>().HasData(
                new Produto
                {
                    Id = 1,
                    Nome = "Gabini® K-29 Premium Headset",
                    Preco = 94.99m,
                    MarcaId = 1,
                    Imagem = "/assets/img/1.svg" // Caminho relativo para o arquivo de imagem
                },
                new Produto
                {
                    Id = 2,
                    Nome = "Gabini® K-30 Premium Headset",
                    Preco = 104.99m,
                    MarcaId = 1,
                    Imagem = "/assets/img/1.svg" // Caminho relativo para o arquivo de imagem
                },
                new Produto
                {
                    Id = 3,
                    Nome = "Gabini® K-31 Premium Headset",
                    Preco = 114.99m,
                    MarcaId = 1,
                    Imagem = "/assets/img/1.svg" // Caminho relativo para o arquivo de imagem
                }
            );
        }
    }
}
