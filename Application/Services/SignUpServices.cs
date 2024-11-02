// File: Application/Services/SignUpService.cs
using Core.DTOs;
using Core.Models;
using Infraestrutura.Repositories;
using BCrypt.Net;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Apresentacao.Services;

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

        public SignUp UpdateProfile(int id, UpdateProfileDTO updateDto)
        {
            var user = _signRepository.GetById(id);
            if (user == null) throw new ArgumentException("Usuário não encontrado.");

            // Validações para CPF e Email se fornecidos
            if (!string.IsNullOrWhiteSpace(updateDto.CPF) && !IsValidCPF(updateDto.CPF))
            {
                throw new ArgumentException("CPF inválido.");
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Email) && !IsValidEmail(updateDto.Email))
            {
                throw new ArgumentException("Email inválido.");
            }

            // Atualização dos campos
            user.Username = !string.IsNullOrWhiteSpace(updateDto.Username) ? updateDto.Username : user.Username;
            user.NomeSocial = updateDto.NomeSocial;
            user.CPF = updateDto.CPF;
            user.Nacionalidade = updateDto.Nacionalidade;
            user.Email = updateDto.Email;
            user.Telefone = updateDto.Telefone;
            user.Sexo = updateDto.Sexo;
            user.Cor = updateDto.Cor;

            // Converte IFormFile Foto para base64, se fornecido
            if (updateDto.Foto != null)
            {
                user.Foto = ConvertImageToBase64(updateDto.Foto.OpenReadStream());
            }

            // Atualizar a senha, se fornecida
            if (!string.IsNullOrWhiteSpace(updateDto.NovaSenha))
            {
                user.Senha = BCrypt.Net.BCrypt.HashPassword(updateDto.NovaSenha);
            }

            _signRepository.Update(user);
            return user;
        }


        private string ConvertImageToBase64(Stream fotoStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                fotoStream.CopyTo(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
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
