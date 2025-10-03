using Application.Dtos;
using Application.Interface;
using Application.Services;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Api_tests
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly Mock<ISenha> _senhaMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _senhaMock = new Mock<ISenha>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _usuarioService = new UsuarioService(_usuarioRepoMock.Object, _senhaMock.Object, _httpContextAccessorMock.Object);
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
            var resultado = await _usuarioService.Registrar(dto, CargoEnum.Usuario);

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

            _usuarioRepoMock.Setup(x => x.ObterPorEmailAsync(dto.Email))
                       .ReturnsAsync(new Usuario(
                           "João",
                           "joao@email.com",
                           CargoEnum.Usuario,
                           [1, 2, 3],
                           [4, 5, 6],
                           true
                       ));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ConflitoException>(() => _usuarioService.Registrar(dto, CargoEnum.Usuario));
            Assert.Equal("Email já cadastrado.", ex.Message);
        }

        [Fact]
        public async Task RegistrarUsuario_DeveFalhar_SeSenhaInvalida()
        {
            var dto = new UsuarioCriacaoDto
            {
                Nome = "Ana",
                Email = "ana@email.com",
                Senha = "123",
                ConfirmarSenha = "123",
                AceitouPoliticaPrivacidade = true
            };

            var ex = await Assert.ThrowsAsync<ValidacaoException>(() => _usuarioService.Registrar(dto, CargoEnum.Usuario));
            Assert.Equal("Senha inválida: deve ter entre 5 e 10 caracteres", ex.Message);
        }

        [Fact]
        public async Task AtualizarUsuario_DeveRetornarSucesso()
        {
            var dto = new UsuarioUpdateDto { Nome = "NovoNome" };
            var usuario = new Usuario("Mateus", "mateus@email.com", CargoEnum.Usuario, new byte[1], new byte[1], true);

            _usuarioRepoMock.Setup(x => x.ObterPorIdAsync(1)).ReturnsAsync(usuario);
            _usuarioRepoMock.Setup(x => x.SalvarAsync()).Returns(Task.CompletedTask);

            var resultado = await _usuarioService.Atualizar(1, dto, 1);

            Assert.NotNull(resultado);
            Assert.Equal("NovoNome", resultado.Nome);
        }

        [Fact]
        public async Task AtualizarUsuario_DeveFalhar_SeNaoEncontrarUsuario()
        {
            var dto = new UsuarioUpdateDto { Nome = "NovoNome" };

            _usuarioRepoMock.Setup(x => x.ObterPorIdAsync(1)).ReturnsAsync((Usuario?)null);

            var ex = await Assert.ThrowsAsync<NaoEncontradoException>(() => _usuarioService.Atualizar(1, dto, 1));
            Assert.Equal("Usuário não encontrado.", ex.Message);
        }

        [Fact]
        public async Task AtualizarUsuario_DeveFalhar_SeNaoAutorizado()
        {
            var dto = new UsuarioUpdateDto { Nome = "NovoNome" };

            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => _usuarioService.Atualizar(1, dto, 2));
            Assert.Equal("Você só pode atualizar seus próprios dados.", ex.Message);
        }

        [Fact]
        public async Task DeletarUsuario_DeveRetornarSucesso()
        {
            var usuario = new Usuario("Mateus", "mateus@email.com", CargoEnum.Usuario, new byte[1], new byte[1], true);

            _usuarioRepoMock.Setup(x => x.ObterPorIdAsync(1)).ReturnsAsync(usuario);
            _usuarioRepoMock.Setup(x => x.SalvarAsync()).Returns(Task.CompletedTask);

            var resultado = await _usuarioService.Deletar(1, 1);

            Assert.True(resultado);
        }

        [Fact]
        public async Task DeletarUsuario_DeveFalhar_SeNaoEncontrarUsuario()
        {
            _usuarioRepoMock.Setup(x => x.ObterPorIdAsync(1)).ReturnsAsync((Usuario?)null);

            var ex = await Assert.ThrowsAsync<NaoEncontradoException>(() => _usuarioService.Deletar(1, 1));
            Assert.Equal("Usuário não encontrado.", ex.Message);
        }

        [Fact]
        public async Task DeletarUsuario_DeveFalhar_SeNaoAutorizado()
        {
            var ex = await Assert.ThrowsAsync<NaoAutorizadoException>(() => _usuarioService.Deletar(1, 2));
            Assert.Equal("Você só pode deletar sua própria conta.", ex.Message);
        }
    }
}
