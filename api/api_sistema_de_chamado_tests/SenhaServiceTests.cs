using api_sistema_de_chamado.Enum;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Services.SenhaService;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
            _configurationMock.Setup(x => x.GetSection("AppSettings:Token").Value).Returns("0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef");

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

        [Fact]
        public void Verifica_SenhaHash_Deve_Retornar_False_Para_SenhaErrada()
        {
            // Arrange
            string senhaCorreta = "senha123";
            string senhaErrada = "senhaErrada";
            _senhaService.CriarSenhaHash(senhaCorreta, out byte[] hash, out byte[] salt);

            // Act
            bool resultado = _senhaService.VerificaSenhaHash(senhaErrada, hash, salt);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void CriarToken_QuandoUsuarioValido_DeveGerarJwtTokenValido()
        {
            // Arrange
            var usuario = new UsuariosModel
            {
                Nome = "Mateus",
                Email = "mateus@email.com",
                Cargo = CargoEnum.Usuario
            };

            // Act
            var tokenGerado = _senhaService.CriarToken(usuario);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(tokenGerado)); // Garante que não é nulo ou vazio

            var tokenHandler = new JwtSecurityTokenHandler();
            var podeLerToken = tokenHandler.CanReadToken(tokenGerado);

            Assert.True(podeLerToken); // Confirma que é um JWT válido
        }

    }
}
