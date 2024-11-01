using Core.Models;
using Infraestrutura.Repositories.Data;
using System.Linq;

namespace Infraestrutura.Repositories
{
    public class TokenRepository
    {
        private readonly EficazContext _context;

        public TokenRepository(EficazContext context)
        {
            _context = context;
        }

        public void SaveToken(Token token)
        {
            _context.Tokens.Add(token);
            _context.SaveChanges();
        }

        public bool IsTokenRevoked(string token)
        {
            return _context.Tokens.Any(t => t.Value == token && t.IsRevoked);
        }

        public void RevokeToken(string token)
        {
            var tokenToRevoke = _context.Tokens.FirstOrDefault(t => t.Value == token);
            if (tokenToRevoke != null)
            {
                tokenToRevoke.IsRevoked = true;
                _context.SaveChanges();
            }
        }
    }
}
