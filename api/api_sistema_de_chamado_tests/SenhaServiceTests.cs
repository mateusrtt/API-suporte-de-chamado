using api_sistema_de_chamado.Services.SenhaService;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api_sistema_de_chamado_tests
{
    public class SenhaServiceTests
    {
        private readonly SenhaService _senhaService;
        private readonly Mock<IConfiguration> _configurationMock;

        public SenhaServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(x => x.GetSection("AppSettings:Token").Value).Returns("supersecretkeyforsigningtoken");

            _senhaService = new SenhaService(_configurationMock.Object);
        }

        [Fact]
        public void CriarSenha_Deve_Gerar_Hash_e_Salt()
        {
            // Arrange
            string senha = "senha123";

            // Act
            _senhaService.CriarSenhaHash(senha, out byte[] hash, out byte[] salt);

            // Assert
            Assert.NotNull(hash);
            Assert.NotNull(salt);
            Assert.NotEmpty(hash);
            Assert.NotEmpty(salt);
        }

        [Fact]
        public void Verifica_SenhaHash_Deve_Retornar_True_Para_SenhaCorreta()
        {
            // Arrange
            string senha = "senha123";
            _senhaService.CriarSenhaHash(senha, out byte[] hash, out byte[] salt);

            // Act
            bool resultado = _senhaService.VerificaSenhaHash(senha, hash, salt);

            // Assert
            Assert.True(resultado);
        }

    }
}
