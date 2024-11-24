using Core.Models;
using Infraestrutura.Repositories;
using Infraestrutura.Repositories.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Integration
{
    public class SignRepositoryIntegrationTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly EficazContext _context;
        private readonly SignRepository _repository;

        public SignRepositoryIntegrationTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<EficazContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new EficazContext(options);
            _context.Database.EnsureCreated();

            _repository = new SignRepository(_context);
        }

        [Fact]
        public void AddSignUp_ShouldSaveSignUpToDatabase()
        {
            var signUp = new SignUp
            {
                Username = "testuser",
                NomeSocial = "Test User",
                CPF = "12345678901",
                Nacionalidade = "Brasil",
                Email = "testuser@example.com",
                Senha = "hashedpassword",
                Telefone = "123456789",
                Sexo = "Feminino",
                Cor = "Branco",
                Foto = null,
                Enderecos = new List<Endereco>
        {
            new Endereco { Logradouro = "Rua A", Complemento = "Apto 101", Cep = "12345000" }
        }
            };

            _repository.Add(signUp);
            var result = _repository.GetById(signUp.Id);

            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
            Assert.Null(result.Foto);
        }

        [Fact]
        public void UpdateSignUp_ShouldModifySignUpInDatabase()
        {
            var signUp = new SignUp
            {
                Username = "initialuser",
                NomeSocial = "Initial User",
                CPF = "98765432100",
                Nacionalidade = "Brasil",
                Email = "initial@example.com",
                Senha = "hashedpassword",
                Telefone = "987654321",
                Sexo = "Masculino",
                Cor = "Negro"
            };

            _repository.Add(signUp);

            signUp.Username = "updateduser";
            _repository.Update(signUp);

            var updatedSignUp = _repository.GetById(signUp.Id);

            Assert.NotNull(updatedSignUp);
            Assert.Equal("updateduser", updatedSignUp.Username);
        }

        public void Dispose()
        {
            _context?.Dispose();
            _connection?.Dispose();
        }
    }
}
