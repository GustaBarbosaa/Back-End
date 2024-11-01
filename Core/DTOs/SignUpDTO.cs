

namespace Core.DTOs
{
    public class SignUpDTO
    {
        public string Username { get; set; }
        public string NomeSocial { get; set; }
        public string CPF { get; set; }
        public string Nacionalidade { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Sexo { get; set; }
        public string Cor { get; set; }
        public string Senha { get; set; }
        public string Foto { get; set; }
        public List<EnderecoDTO> Enderecos { get; set; }
    }
}
