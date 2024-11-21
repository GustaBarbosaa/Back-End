// File: Application/Services/SignUpService.cs
using Core.DTOs;
using Core.Models;
using Infraestrutura.Repositories;
using BCrypt.Net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Apresentacao.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class SignUpService
    {
        private readonly SignRepository _signRepository;
        private readonly TokenService _tokenService;

        public SignUpService(SignRepository signRepository, TokenService tokenService)
        {
            _signRepository = signRepository;
            _tokenService = tokenService;
        }

        public (SignUp, string) CreateSignUp(SignUpDTO signUpDto)
        {
            ValidateSignUpData(signUpDto);

            string fotoBase64 = null;
            if (signUpDto.Foto != null)
            {
                // Usa OpenReadStream() para converter IFormFile para Stream
                fotoBase64 = ConvertImageToBase64(signUpDto.Foto.OpenReadStream());
            }

            var signUp = new SignUp
            {
                Username = signUpDto.Username,
                NomeSocial = signUpDto.NomeSocial,
                CPF = signUpDto.CPF,
                Nacionalidade = signUpDto.Nacionalidade,
                Email = signUpDto.Email,
                Telefone = signUpDto.Telefone,
                Sexo = signUpDto.Sexo,
                Cor = signUpDto.Cor,
                Senha = BCrypt.Net.BCrypt.HashPassword(signUpDto.Senha),
                Foto = fotoBase64,
                Enderecos = signUpDto.Enderecos.Select(e => new Endereco
                {
                    Logradouro = e.Logradouro,
                    Complemento = e.Complemento,
                    Cep = e.Cep
                }).ToList()
            };

            _signRepository.Add(signUp);

            var token = _tokenService.GenerateToken(signUp);
            return (signUp, token);
        }

        public (SignUp, string) Authenticate(string email, string senha)
        {
            var user = _signRepository.GetByEmail(email);
            if (user == null || !VerifyPassword(senha, user.Senha))
            {
                return (null, null);
            }

            var token = _tokenService.GenerateToken(user);
            return (user, token);
        }

        public SignUp GetSignUpById(int id)
        {
            return _signRepository.GetById(id);
        }

        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }

        // Adicione dentro da classe SignUpService

        public void UpdateProfile(int id, UpdateProfileDTO updateDto)
        {
            var existingUser = _signRepository.GetById(id);

            if (existingUser == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            // Atualizar apenas os campos não nulos e diferentes de "string"
            if (!string.IsNullOrWhiteSpace(updateDto.Username) && updateDto.Username != "string")
                existingUser.Username = updateDto.Username;

            if (!string.IsNullOrWhiteSpace(updateDto.NomeSocial) && updateDto.NomeSocial != "string")
                existingUser.NomeSocial = updateDto.NomeSocial;

            if (!string.IsNullOrWhiteSpace(updateDto.CPF) && updateDto.CPF != "string")
                existingUser.CPF = updateDto.CPF;

            if (!string.IsNullOrWhiteSpace(updateDto.Nacionalidade) && updateDto.Nacionalidade != "string")
                existingUser.Nacionalidade = updateDto.Nacionalidade;

            if (!string.IsNullOrWhiteSpace(updateDto.Email) && updateDto.Email != "string")
                existingUser.Email = updateDto.Email;

            if (!string.IsNullOrWhiteSpace(updateDto.Telefone) && updateDto.Telefone != "string")
                existingUser.Telefone = updateDto.Telefone;

            if (!string.IsNullOrWhiteSpace(updateDto.Sexo) && updateDto.Sexo != "string")
                existingUser.Sexo = updateDto.Sexo;

            if (!string.IsNullOrWhiteSpace(updateDto.Cor) && updateDto.Cor != "string")
                existingUser.Cor = updateDto.Cor;

            // Atualizar senha apenas se for válida
            if (!string.IsNullOrWhiteSpace(updateDto.NovaSenha) && updateDto.NovaSenha != "string")
            {
                if (updateDto.NovaSenha.Length < 6)
                    throw new ArgumentException("Senha inválida.");

                existingUser.Senha = BCrypt.Net.BCrypt.HashPassword(updateDto.NovaSenha);
            }

            // Atualizar foto se fornecida
            if (updateDto.Foto != null)
            {
                existingUser.Foto = ConvertImageToBase64(updateDto.Foto.OpenReadStream());
            }

            _signRepository.Update(existingUser);
        }





        // Método para salvar a foto e retornar o caminho/URL
        private string ConvertToPath(IFormFile file)
        {
            // Simulação de upload de arquivo - substitua por lógica real de armazenamento
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine("wwwroot/uploads", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return $"/uploads/{fileName}";
        }


        private string ConvertImageToBase64(Stream fotoStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                fotoStream.CopyTo(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 8 &&
                   Regex.IsMatch(password, @"[A-Z]") &&
                   Regex.IsMatch(password, @"[a-z]") &&
                   Regex.IsMatch(password, @"\d") &&
                   Regex.IsMatch(password, @"[^\w\d\s:]");
        }


        private void ValidateSignUpData(SignUpDTO signUpDto)
        {
            if (!IsValidCPF(signUpDto.CPF))
            {
                throw new ArgumentException("CPF inválido.");
            }

            if (!IsValidEmail(signUpDto.Email))
            {
                throw new ArgumentException("Email inválido.");
            }

            if (_signRepository.ExistsByCPF(signUpDto.CPF))
            {
                throw new ArgumentException("CPF já cadastrado.");
            }

            if (_signRepository.ExistsByEmail(signUpDto.Email))
            {
                throw new ArgumentException("Email já cadastrado.");
            }
        }

        private bool IsValidCPF(string cpf)
        {
            return Regex.IsMatch(cpf, @"^\d{11}$");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
