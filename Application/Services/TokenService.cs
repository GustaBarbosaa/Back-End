using Core.Models;
using Infraestrutura.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Apresentacao.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly TokenRepository _tokenRepository;

        public TokenService(IConfiguration configuration, TokenRepository tokenRepository)
        {
            _configuration = configuration;
            _tokenRepository = tokenRepository;
        }

        public string GenerateToken(SignUp user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("SignUpId", user.Id.ToString())  
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _tokenRepository.SaveToken(new Token
            {
                Value = tokenString,
                CreatedAt = DateTime.UtcNow,
                Expiration = tokenDescriptor.Expires,
                SignUpId = user.Id  
            });

            return tokenString;
        }



        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                if (_tokenRepository.IsTokenRevoked(token))
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void RevokeToken(string token)
        {
            _tokenRepository.RevokeToken(token);
        }
    }
}
