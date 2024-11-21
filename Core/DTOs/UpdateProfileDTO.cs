using Microsoft.AspNetCore.Http;

namespace Core.DTOs
{
    public class UpdateProfileDTO
    {
        public string? Username { get; set; }
        public string? NomeSocial { get; set; }
        public string? CPF { get; set; }
        public string? Nacionalidade { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Sexo { get; set; }
        public string? Cor { get; set; }
        public IFormFile? Foto { get; set; } // Opcional
        public string? NovaSenha { get; set; } // Agora opcional
    }
}
