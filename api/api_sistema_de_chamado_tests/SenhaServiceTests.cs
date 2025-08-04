using api_sistema_de_chamado.Enum;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Services.SenhaService;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        [Theory]
        [InlineData(1, "Mateus", "mateus@email.com", CargoEnum.Administrador, "Administrador")]
        [InlineData(2, "João", "joao@email.com", CargoEnum.Usuario, "Usuario")]
        public void CriarToken_QuandoUsuarioValido_DeveGerarJwtTokenValido(
            int id, string nome, string email, CargoEnum cargo, string cargoEsperado)
        {
            // Arrange
            var usuario = new UsuariosModel
            {
                Id = id,
                Nome = nome,
                Email = email,
                Cargo = cargo
            };

            // Act
            var tokenGerado = _senhaService.CriarToken(usuario);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(tokenGerado)); 

            var tokenHandler = new JwtSecurityTokenHandler();
            Assert.True(tokenHandler.CanReadToken(tokenGerado)); 

            var token = tokenHandler.ReadJwtToken(tokenGerado);

            var roleClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            Assert.NotNull(roleClaim);
            Assert.Equal(cargoEsperado, roleClaim!.Value); 

            var idClaim = token.Claims.FirstOrDefault(c => c.Type == "Id");
            Assert.NotNull(idClaim);
            Assert.Equal(id.ToString(), idClaim!.Value); 
        }
    }
}
