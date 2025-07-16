using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Repositories.Usuario;
using api_sistema_de_chamado.Services.AuthService;
using api_sistema_de_chamado.Services.SenhaService;
using Moq;

namespace api_sistema_de_chamado_tests
{
    public class AuthServiceTests
    {

        private readonly Mock<IUsuarioRepository> _usuarioRepoMock;
        private readonly Mock<ISenhaInterface> _senhaMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _senhaMock = new Mock<ISenhaInterface>();
            _authService = new AuthService(_usuarioRepoMock.Object, _senhaMock.Object);
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
                ConfirmarSenha = "123456"
            };

            _usuarioRepoMock.Setup(x => x.ExisteUsuarioOuEmailAsync(dto.Nome, dto.Email))
                .ReturnsAsync(true);

            _senhaMock.Setup(x => x.CriarSenhaHash(dto.Senha, out It.Ref<byte[]>.IsAny, out It.Ref<byte[]>.IsAny))
                .Callback((string s, out byte[] senhaHash, out byte[] senhaSalt) =>
                {
                    senhaHash = new byte[1];
                    senhaSalt = new byte[1];
                });

            _usuarioRepoMock.Setup(x => x.AdicionarAsync(It.IsAny<UsuariosModel>())).Returns(Task.CompletedTask);
            _usuarioRepoMock.Setup(x => x.SalvarAsync()).Returns(Task.CompletedTask);

            // Act
            var resultado = await _authService.RegistrarUsuario(dto);

            // Assert
            Assert.True(resultado.Status); // Deve retornar sucesso
            Assert.Equal("Usuário cadastrado com sucesso!", resultado.Mensagem);

        }
    }
}