using Application.Dtos;
using Application.Services;
using Application.Interface;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Api_tests
{
    public class AuthServiceTests
    {

        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly Mock<ISenha> _senhaMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IAcessoLogRepository> _acessoLogRepoMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _senhaMock = new Mock<ISenha>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _acessoLogRepoMock = new Mock<IAcessoLogRepository>();

            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = new ServiceCollection()
                .AddSingleton(_acessoLogRepoMock.Object)
                .BuildServiceProvider();

            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(new DefaultHttpContext());

            _authService = new AuthService(
              _usuarioRepoMock.Object,
              _senhaMock.Object,
              _httpContextAccessorMock.Object,
              _acessoLogRepoMock.Object
          );
        }

        [Fact]
        public async Task RegistrarUsuario_DeveRetornarSucesso_SeDadosValidos()
        {
            // Arrange
            var dto = new UsuarioCriacaoDto
            {
                Nome = "Mateus",
                Email = "mateus@email.com",
                Senha = "123456",
                ConfirmarSenha = "123456",
                AceitouPoliticaPrivacidade = true
            };

            _usuarioRepoMock.Setup(x => x.ExisteUsuarioOuEmailAsync(dto.Nome, dto.Email)).ReturnsAsync(true);

            _senhaMock.Setup(x => x.CriarSenhaHash(dto.Senha, out It.Ref<byte[]>.IsAny, out It.Ref<byte[]>.IsAny))
                .Callback((string s, out byte[] senhaHash, out byte[] senhaSalt) =>
                {
                    senhaHash = new byte[1];
                    senhaSalt = new byte[1];
                });

            _usuarioRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<Usuario>())).Returns(Task.CompletedTask);
            _usuarioRepoMock.Setup(x => x.SalvarAsync()).Returns(Task.CompletedTask);

            _acessoLogRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<AcessoLog>()))
                             .ReturnsAsync((AcessoLog log) => log); // retorna o próprio log que foi passado

            _acessoLogRepoMock.Setup(x => x.SalvarAsync())
                              .ReturnsAsync(true);

            // Act
            var resultado = await _authService.Registrar(dto, CargoEnum.Usuario);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(string.Empty, resultado.Nome);
            Assert.Equal(string.Empty, resultado.Email);
        }

        [Fact]
        public async Task RegistrarUsuario_DeveFalhar_SeUsuarioExistir()
        {
            //Arrange
            var dto = new UsuarioCriacaoDto
            {
                Nome = "João",
                Email = "joao@email.com",
                Senha = "123456",
                ConfirmarSenha = "123456",
                AceitouPoliticaPrivacidade = true
            };

            var usuarioExistente = new Usuario(
                "João",
                "joao@email.com",
                CargoEnum.Usuario,
                new byte[] { 1, 2, 3 },   // hash dummy
                new byte[] { 4, 5, 6 },   // salt dummy
                true
            );

            _usuarioRepoMock.Setup(x => x.ObterPorEmailAsync(dto.Email))
                       .ReturnsAsync(new Usuario(
                           "João",
                           "joao@email.com",
                           CargoEnum.Usuario,
                           new byte[] { 1, 2, 3 },
                           new byte[] { 4, 5, 6 },
                           true
                       ));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _authService.Registrar(dto, CargoEnum.Usuario));
            Assert.Equal("Email já cadastrado.", ex.Message);
        }

        [Fact]
        public async Task Login_DeveRetornarToken_SeLoginValido()
        {
            // Arrange
            var dto = new UsuarioLoginDto
            {
                Email = "teste@email.com",
                Senha = "senha"
            };

            var usuario = new Usuario("Teste", dto.Email, CargoEnum.Usuario, new byte[1], new byte[1], true);

            _usuarioRepoMock.Setup(x => x.ObterPorEmailAsync(dto.Email)).ReturnsAsync(usuario);
            _senhaMock.Setup(x => x.VerificaSenhaHash(dto.Senha, usuario.SenhaHash, usuario.SenhaSalt)).Returns(true);
            _senhaMock.Setup(x => x.CriarToken(usuario)).Returns("fake-jwt-token");

            // Act
            var resultado = await _authService.Login(dto);

            // Assert
            Assert.Equal("fake-jwt-token", resultado);
        }

        [Fact]
        public async Task Login_Deve_Falhar_Se_Usuario_Nao_Encontrado()
        {
            // Arrange
            var dto = new UsuarioLoginDto { Email = "teste@email.com", Senha = "senha" };

            _usuarioRepoMock.Setup(x => x.ObterPorEmailAsync(dto.Email)).ReturnsAsync((Usuario?)null);

            // Act Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _authService.Login(dto));
            Assert.Equal("Email inválido!", ex.Message);
        }

        [Fact]
        public async Task Login_DeveFalhar_SeSenhaIncorreta()
        {
            // Arrange
            var dto = new UsuarioLoginDto
            {
                Email = "teste@email.com",
                Senha = "senhaerrada"
            };

            var usuario = new Usuario(
                "Teste",
                dto.Email,
                CargoEnum.Usuario,
                new byte[1],  
                new byte[1],
                true
            );

            _usuarioRepoMock.Setup(x => x.ObterPorEmailAsync(dto.Email)).ReturnsAsync(usuario);
            _senhaMock.Setup(x => x.VerificaSenhaHash(dto.Senha, usuario.SenhaHash, usuario.SenhaSalt)).Returns(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => _authService.Login(dto));
            Assert.Equal("Senha inválida!", ex.Message);
        }
    }
}