namespace Core.Models
{
    public class Produto
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public decimal? Preco { get; set; }
        public int? MarcaId { get; set; }
        public string? Imagem { get; set; }
        public string? ImagemHover { get; set; }

        public Marca Marca { get; set; }
    }

}
