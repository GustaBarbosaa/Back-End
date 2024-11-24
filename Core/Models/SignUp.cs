namespace Core.Models
{
    public class SignUp
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string NomeSocial { get; set; }
        public string CPF { get; set; }
        public string Nacionalidade { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Telefone { get; set; }
        public string? Foto { get; set; }  
        public string Sexo { get; set; }
        public string Cor { get; set; }
        public List<Endereco> Enderecos { get; set; }
        public List<Token> Tokens { get; set; }
    }
}
