using Core.Models;

public class Marca
{
    public int Id { get; set; }           
    public string Nome { get; set; }      

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}