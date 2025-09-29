using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace Api_tests
{
    public class AcessoLogServiceTests
    {
        private readonly Mock<IAcessoLogRepository> _logRepoMock;
        private readonly Mock<ILogger<AcessoLogService>> _loggerMock;
        private readonly AcessoLogService _acessoLogService;

        public AcessoLogServiceTests()
        {
            _logRepoMock = new Mock<IAcessoLogRepository>();
            _loggerMock = new Mock<ILogger<AcessoLogService>>();
            _acessoLogService = new AcessoLogService(_logRepoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Registrar_DeveChamarRepositorio_QuandoSucesso()
        {
            // Arrange
            int? usuarioId = 1;
            string nome = "Teste";
            string email = "teste@email.com";
            string ip = "127.0.0.1";
            string metodo = "GET";
            string url = "/api/teste";

            // Act
            await _acessoLogService.Registrar(usuarioId, nome, email, ip, metodo, url);

            // Assert
            _logRepoMock.Verify(x => x.AdicionarAsync(It.Is<AcessoLog>(
                log =>
                    log.UsuarioId == usuarioId &&
                    log.Nome == nome &&
                    log.Email == email &&
                    log.IpAcesso == ip &&
                    log.MetodoHttp == metodo &&
                    log.Endpoint == url
            )), Times.Once);

            _logRepoMock.Verify(x => x.SalvarAsync(), Times.Once);
        }

        [Fact]
        public async Task Registrar_NaoLancaExcecao_QuandoRepositorioFalha()
        {
            // Arrange
            _logRepoMock
                .Setup(x => x.AdicionarAsync(It.IsAny<AcessoLog>()))
                .ThrowsAsync(new Exception("Erro no banco"));

            // Act
            var exception = await Record.ExceptionAsync(() =>
                _acessoLogService.Registrar(1, "Teste", "teste@email.com", "127.0.0.1", "POST", "/api/erro")
            );

            // Assert → não deve propagar exceção
            Assert.Null(exception);

            // Deve logar o erro
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Falha ao registrar log de acesso")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                Times.Once
            );
        }
    }
}