// File: Infraestrutura/Repositories/SignRepository.cs
using Core.Models;
using System.Linq;
using Infraestrutura.Repositories.Data;

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

        public SignUp GetByEmail(string email)
        {
            return _context.SignUps.FirstOrDefault(s => s.Email == email);
        }

        public bool ExistsByCPF(string cpf)
        {
            return _context.SignUps.Any(s => s.CPF == cpf);
        }

        public bool ExistsByEmail(string email)
        {
            return _context.SignUps.Any(s => s.Email == email);
        }

        // Método para atualizar o registro de SignUp no banco de dados
        public void Update(SignUp signUp)
        {
            var existingUser = _context.SignUps.FirstOrDefault(s => s.Id == signUp.Id);
            if (existingUser == null)
            {
                throw new ArgumentException("Usuário não encontrado para atualização.");
            }

            // Atualizar apenas os campos alterados para evitar substituições desnecessárias
            _context.Entry(existingUser).CurrentValues.SetValues(signUp);
            _context.SaveChanges();
        }

    }
}
