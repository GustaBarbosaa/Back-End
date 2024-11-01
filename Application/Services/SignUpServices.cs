using Core.DTOs;
using Core.Models;
using Infraestrutura.Repositories;
using BCrypt.Net;
using System.Text.RegularExpressions;
using Apresentacao.Services;
using System.IO;
using System;

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

            string fotoBase64 = null;
            if (signUpDto.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    signUpDto.Foto.CopyTo(memoryStream);
                    fotoBase64 = Convert.ToBase64String(memoryStream.ToArray());
                }
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

        public SignUp GetSignUpById(int id)
        {
            return _signRepository.GetById(id);
        }

        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
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
