using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Enums;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Repositories.AcessoLog;
using api_sistema_de_chamado.Repositories.Usuario;
using api_sistema_de_chamado.Services.AuthService;
using api_sistema_de_chamado.Services.SenhaService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace api_sistema_de_chamado_tests
{
    public class AuthServiceTests
    {

        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly Mock<ISenhaInterface> _senhaMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IAcessoLogRepository> _acessoLogRepoMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _senhaMock = new Mock<ISenhaInterface>();
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

            _usuarioRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<UsuariosModel>())).Returns(Task.CompletedTask);
            _usuarioRepoMock.Setup(x => x.SalvarAsync()).Returns(Task.CompletedTask);

            _acessoLogRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<AcessoLogModel>()))
                             .ReturnsAsync((AcessoLogModel log) => log); // retorna o próprio log que foi passado

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

            var usuarioExistente = new UsuariosModel(
                "João",
                "joao@email.com",
                CargoEnum.Usuario,
                new byte[] { 1, 2, 3 },   // hash dummy
                new byte[] { 4, 5, 6 },   // salt dummy
                true
            );

            _usuarioRepoMock.Setup(x => x.ObterPorEmailAsync(dto.Email))
                       .ReturnsAsync(new UsuariosModel(
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

            var usuario = new UsuariosModel("Teste", dto.Email, CargoEnum.Usuario, new byte[1], new byte[1], true);

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

            _usuarioRepoMock.Setup(x => x.ObterPorEmailAsync(dto.Email)).ReturnsAsync((UsuariosModel?)null);

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

            var usuario = new UsuariosModel(
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