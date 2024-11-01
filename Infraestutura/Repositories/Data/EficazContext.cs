using Microsoft.EntityFrameworkCore;
using Core.Models;

namespace Infraestrutura.Repositories.Data
{
    public class EficazContext : DbContext
    {
        public EficazContext(DbContextOptions<EficazContext> options) : base(options) { }

        public DbSet<SignUp> SignUps { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
