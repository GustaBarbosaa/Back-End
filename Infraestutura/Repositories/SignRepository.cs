using Core.Models;
using Infraestrutura.Repositories.Data;
using System.Linq;

namespace Infraestrutura.Repositories
{
    public class SignRepository
    {
        private readonly EficazContext _context;

        public SignRepository(EficazContext context)
        {
            _context = context;
        }

        public SignUp Add(SignUp signUp)
        {
            _context.SignUps.Add(signUp);
            _context.SaveChanges();
            return signUp;
        }

        public SignUp GetById(int id)
        {
            return _context.SignUps.FirstOrDefault(s => s.Id == id);
        }

        public bool ExistsByCPF(string cpf)
        {
            return _context.SignUps.Any(s => s.CPF == cpf);
        }

        public bool ExistsByEmail(string email)
        {
            return _context.SignUps.Any(s => s.Email == email);
        }
    }
}
