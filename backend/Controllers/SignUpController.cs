
using Core.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Apresentacao.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignUpController : ControllerBase
    {
        private readonly SignUpService _signUpService;

        public SignUpController(SignUpService signUpService)
        {
            _signUpService = signUpService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSignUp([FromForm] SignUpDTO signUpDto)
        {
            try
            {
                var (signUp, token) = _signUpService.CreateSignUp(signUpDto);

                return CreatedAtAction(nameof(GetSignUpById), new { id = signUp.Id }, new
                {
                    User = new
                    {
                        signUp.Id,
                        signUp.Username,
                        signUp.NomeSocial,
                        signUp.CPF,
                        signUp.Nacionalidade,
                        signUp.Email,
                        signUp.Telefone,
                        signUp.Sexo,
                        signUp.Cor,
                        signUp.Foto,
                        signUp.Enderecos
                    },
                    Token = token
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var (user, token) = _signUpService.Authenticate(loginDto.Email, loginDto.Senha);

                if (user == null)
                {
                    return Unauthorized(new { error = "Email ou senha incorretos." });
                }

                return Ok(new
                {
                    User = new
                    {
                        user.Id,
                        user.Username,
                        user.Email
                    },
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetSignUpById(int id)
        {
            var signUp = _signUpService.GetSignUpById(id);
            if (signUp == null) return NotFound(new { error = "Usuário não encontrado." });

            return Ok(new
            {
                signUp.Id,
                signUp.Username,
                signUp.NomeSocial,
                signUp.CPF,
                signUp.Nacionalidade,
                signUp.Email,
                signUp.Telefone,
                signUp.Sexo,
                signUp.Cor,
                signUp.Foto,
                signUp.Enderecos
            });
        }

        [HttpGet("profile/{id}")]
        public IActionResult GetUserProfile(int id)
        {
            var user = _signUpService.GetSignUpById(id);
            if (user == null) return NotFound(new { error = "Usuário não encontrado." });

            return Ok(new
            {
                user.Username,
                Foto = user.Foto
            });
        }

        [HttpPut("editar-perfil/{id}")]
        public IActionResult UpdateProfile(int id, [FromForm] UpdateProfileDTO updateDto)
        {
            try
            {
                // Verifica se o usuário existe antes de tentar atualizar
                var existingUser = _signUpService.GetSignUpById(id);
                if (existingUser == null)
                {
                    return NotFound(new { error = "Usuário não encontrado para atualização." });
                }

                var updatedUser = _signUpService.UpdateProfile(id, updateDto);

                return Ok(new
                {
                    User = new
                    {
                        updatedUser.Id,
                        updatedUser.Username,
                        updatedUser.NomeSocial,
                        updatedUser.CPF,
                        updatedUser.Nacionalidade,
                        updatedUser.Email,
                        updatedUser.Telefone,
                        updatedUser.Sexo,
                        updatedUser.Cor,
                        updatedUser.Foto,
                        updatedUser.Senha,
                    }
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno ao processar a solicitação." });
            }
        }
    }
}

