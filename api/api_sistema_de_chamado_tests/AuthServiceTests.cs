using Application.Dtos;
using Application.Services;
using Application.Interface;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Domain.Exceptions;

namespace Api_tests
{
    public class AuthServiceTests
    {

        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly Mock<ISenha> _senhaMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _senhaMock = new Mock<ISenha>();

            _authService = new AuthService(
              _usuarioRepoMock.Object,
              _senhaMock.Object
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

            // Act
            var resultado = await _authService.Registrar(dto, CargoEnum.Usuario);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(dto.Nome, resultado.Nome);
            Assert.Equal(dto.Email.ToLower(), resultado.Email);
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
                [1, 2, 3],
                [4, 5, 6],
                true
            );

            _usuarioRepoMock.Setup(x => x.ObterPorEmailAsync(dto.Email))
                       .ReturnsAsync(new Usuario(
                           "João",
                           "joao@email.com",
                           CargoEnum.Usuario,
                           [1, 2, 3],
                           [ 4, 5, 6 ],
                           true
                       ));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ConflitoException>(() => _authService.Registrar(dto, CargoEnum.Usuario));
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
            Assert.NotNull(resultado);
            Assert.Equal("fake-jwt-token", resultado.Token);
            Assert.Equal(usuario.Nome, resultado.Usuario.Nome);
            Assert.Equal(usuario.Email.ToLower(), resultado.Usuario.Email);
        }

        [Fact]
        public async Task Login_Deve_Falhar_Se_Usuario_Nao_Encontrado()
        {
            // Arrange
            var dto = new UsuarioLoginDto { Email = "teste@email.com", Senha = "senha" };

            _usuarioRepoMock.Setup(x => x.ObterPorEmailAsync(dto.Email)).ReturnsAsync((Usuario?)null);

            // Act Assert
            var ex = await Assert.ThrowsAsync<NaoEncontradoException>(() => _authService.Login(dto));
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
            var ex = await Assert.ThrowsAsync<ValidacaoException>(() => _authService.Login(dto));
            Assert.Equal("Senha inválida!", ex.Message);
        }
    }
}