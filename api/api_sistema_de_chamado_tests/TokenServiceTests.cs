using Application.Configurations;
using Application.Services;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api_tests
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            var jwtConfig = Options.Create(new JwtConfig
            {
                Token = "0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef",
                ExpiracaoHoras = 24
            });

            _tokenService = new TokenService(jwtConfig);
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
            var tokenGerado = _tokenService.CriarToken(usuario);

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