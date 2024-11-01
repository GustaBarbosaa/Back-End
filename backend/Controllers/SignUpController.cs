using Core.DTOs;
using Core.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateSignUp(SignUpDTO signUpDto)
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

        [HttpGet("{id}")]
        public IActionResult GetSignUpById(int id)
        {
            var signUp = _signUpService.GetSignUpById(id);
            if (signUp == null) return NotFound();

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
    }
}
