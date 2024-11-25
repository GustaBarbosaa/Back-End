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
        private readonly TokenRepository _tokenRepository;
        private readonly byte[] _secretKey;

        public TokenService(IConfiguration configuration, TokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;

            // Securely fetch the secret key from environment variables
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }
            _secretKey = Encoding.ASCII.GetBytes(secretKey);
        }

        public string GenerateToken(SignUp user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha256Signature)
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

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_secretKey),
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
