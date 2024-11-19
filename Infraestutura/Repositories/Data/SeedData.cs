using Microsoft.EntityFrameworkCore;
using Core.Models;
using System.IO;

namespace Infraestrutura.Repositories.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Método auxiliar para carregar e converter a imagem para Base64
            string ConvertImageToBase64(string relativePath)
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
                if (!File.Exists(fullPath))
                {
                    Console.WriteLine($"Imagem não encontrada: {fullPath}");
                    return null; // Retorna nulo se o arquivo não existir
                }

                var fileBytes = File.ReadAllBytes(fullPath);
                return Convert.ToBase64String(fileBytes);
            }

            // Caminho da imagem hover
            string hoverImageBase64 = ConvertImageToBase64("assets/img/2.svg");

            // Adicionar marcas
            modelBuilder.Entity<Marca>().HasData(
                new Marca { Id = 1, Nome = "Gabini" }
            );

            // Adicionar produtos com imagens e imagens de hover em Base64
            modelBuilder.Entity<Produto>().HasData(
                new Produto
                {
                    Id = 1,
                    Nome = "Gabini® K-29 Premium Headset",
                    Preco = 94.99m,
                    MarcaId = 1,
                    Imagem = ConvertImageToBase64("assets/img/1.svg"),
                    ImagemHover = hoverImageBase64
                },
                new Produto
                {
                    Id = 2,
                    Nome = "Gabini® K-30 Premium Headset",
                    Preco = 104.99m,
                    MarcaId = 1,
                    Imagem = ConvertImageToBase64("assets/img/1.svg"),
                    ImagemHover = hoverImageBase64
                },
                new Produto
                {
                    Id = 3,
                    Nome = "Gabini® K-31 Premium Headset",
                    Preco = 114.99m,
                    MarcaId = 1,
                    Imagem = ConvertImageToBase64("assets/img/1.svg"),
                    ImagemHover = hoverImageBase64
                }
            );
        }
    }
}
