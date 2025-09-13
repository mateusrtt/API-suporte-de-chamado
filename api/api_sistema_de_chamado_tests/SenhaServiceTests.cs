using Application.Services;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Api_tests
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

        [Theory]
        [InlineData("senha123")]
        [InlineData("minhaSenha@2025")]
        [InlineData("Senha!Com#Caracteres$Especiais")]
        [InlineData("1234567890")]
        [InlineData("senha com espaço")]
        public void CriarSenha_Deve_Gerar_Hash_e_Salt(string senha)
        {
            // Act
            _senhaService.CriarSenhaHash(senha, out byte[] hash, out byte[] salt);

            // Assert
            Assert.NotNull(hash);
            Assert.NotNull(salt);
            Assert.NotEmpty(hash);
            Assert.NotEmpty(salt);
            Assert.NotEqual(hash, salt);
            Assert.Equal(64, hash.Length);   
            Assert.Equal(128, salt.Length);
        }

        [Theory]
        [InlineData("senha123")]
        [InlineData("TesteSeguranca")]
        [InlineData("123456789")]
        [InlineData("SenhaCom!Simbolo")]
        public void Verifica_SenhaHash_Deve_Retornar_True_Para_SenhaCorreta(string senha)
        {
            // Arrange
            _senhaService.CriarSenhaHash(senha, out byte[] hash, out byte[] salt);

            // Act
            bool resultado = _senhaService.VerificaSenhaHash(senha, hash, salt);

            // Assert
            Assert.True(resultado);
        }

        [Theory]
        [InlineData("senha123", "outraSenha")]
        [InlineData("minhaSenhaSegura", "senha123")]
        [InlineData("senha com espaço", "senha sem espaço")]
        [InlineData("senhaMAIUSCULA", "senhamaIuscula")]
        public void Verifica_SenhaHash_Deve_Retornar_False_Para_SenhaErrada(string senhaCorreta, string senhaErrada)
        {
            // Arrange
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
            var usuario = new Usuario(
                nome,
                email,
                cargo,
                senhaHash: new byte[1],   
                senhaSalt: new byte[1],
                true
            );

            var idProperty = typeof(Usuario).GetProperty("Id");
            idProperty!.SetValue(usuario, id);

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
