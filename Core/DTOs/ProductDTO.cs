using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int MarcaId { get; set; }
        public string Imagem { get; set; }
    }
}
