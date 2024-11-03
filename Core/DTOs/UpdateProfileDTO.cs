// File: Core/DTOs/UpdateProfileDTO.cs
using Microsoft.AspNetCore.Http;

namespace Core.DTOs
{
    public class UpdateProfileDTO
    {
        public string Username { get; set; }
        public string NomeSocial { get; set; }
        public string CPF { get; set; }
        public string Nacionalidade { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Sexo { get; set; }
        public string Cor { get; set; }

        // Alterado de string para IFormFile
        public IFormFile Foto { get; set; }
        public string NovaSenha { get; set; }
    }
}

