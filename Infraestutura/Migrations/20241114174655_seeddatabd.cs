using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infraestutura.Migrations
{
    /// <inheritdoc />
    public partial class seeddatabd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Marcas",
                columns: new[] { "Id", "Nome" },
                values: new object[] { 1, "Gabini" });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "MarcaId", "Nome", "Preco" },
                values: new object[,]
                {
                    { 1, 1, "Gabini® K-29 Premium Headset", 94.99m },
                    { 2, 1, "Gabini® K-30 Premium Headset", 104.99m },
                    { 3, 1, "Gabini® K-31 Premium Headset", 114.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Marcas",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
