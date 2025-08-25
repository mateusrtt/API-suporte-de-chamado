using api_sistema_de_chamado.Dtos;
using api_sistema_de_chamado.Enum;
using api_sistema_de_chamado.Models;
using api_sistema_de_chamado.Repositories.Chamado;
using api_sistema_de_chamado.Services.ChamadoService;
using api_sistema_de_chamado.Services.OpenAiService;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace api_sistema_de_chamado_tests
{
    public class ChamadoServiceTests
    {
        private readonly Mock<IChamadoRepository> _chamadoRepositoryMock;
        private readonly Mock<IOpenAiInterface> _openAiServiceMock;
        private readonly ChamadoService _service;

        public ChamadoServiceTests()
        {
            _chamadoRepositoryMock = new Mock<IChamadoRepository>();
            _openAiServiceMock = new Mock<IOpenAiInterface>();
            _service = new ChamadoService(_chamadoRepositoryMock.Object, _openAiServiceMock.Object);
        }

        [Fact]
        public async Task FazerChamadoAsync_DeveCriarChamadoComSucesso()
        {
            // Arrange
            var dto = new FazerChamadoDto { Descricao = "Problema no sistema" };
            int usuarioId = 1;

            _openAiServiceMock
                .Setup(s => s.ObterPrioridadePorDescricaoAsync(dto.Descricao))
                .ReturnsAsync((int)PrioridadeEnum.Alta);

            _chamadoRepositoryMock.Setup(r => r.AdicionarAsync(It.IsAny<ChamadoModel>()));
            _chamadoRepositoryMock.Setup(r => r.SalvarAsync());

            // Act
            var result = await _service.FazerChamadoAsync(dto, usuarioId);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Chamado criado com sucesso!", result.Mensagem);
            Assert.NotNull(result.Dados);
            Assert.Equal(dto.Descricao, result.Dados!.Descricao);
        }

        [Fact]
        public async Task ResponderChamadoAsync_DeveRetornarErro_SeNaoEncontrarChamado()
        {
            // Arrange
            int chamadoId = 1;
            var dto = new ResponderChamadoDto { Resposta = "Resolvido" };

            _chamadoRepositoryMock.Setup(r => r.ObterIdAsync(chamadoId))
                                  .ReturnsAsync((ChamadoModel?)null);

            // Act
            var result = await _service.ResponderChamadoAsync(chamadoId, dto);

            // Assert
            Assert.False(result.Status);
            Assert.Equal("Chamado não encontrado.", result.Mensagem);
        }

        [Fact]
        public async Task ResponderChamadoAsync_DeveResponderChamado()
        {
            // Arrange
            int chamadoId = 1;
            var dto = new ResponderChamadoDto { Resposta = "Resolvido" };

            var chamado = new ChamadoModel("Erro de login", 1, PrioridadeEnum.Baixa);

            _chamadoRepositoryMock.Setup(r => r.ObterIdAsync(chamadoId))
                                  .ReturnsAsync(chamado);

            _chamadoRepositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<ChamadoModel>()));
            _chamadoRepositoryMock.Setup(r => r.SalvarAsync());

            // Act
            var result = await _service.ResponderChamadoAsync(chamadoId, dto);

            // Assert
            Assert.True(result.Status);
            Assert.Equal("Chamado respondido com sucesso!", result.Mensagem);
            Assert.NotNull(result.Dados);
            Assert.Equal(dto.Resposta, result.Dados!.Resposta);
            Assert.Equal(StatusChamadoEnum.Respondido, result.Dados!.Status);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ListarChamadosPorUsuarioAsync_DeveRetornarChamados(int usuarioId)
        {
            // Arrange
            var chamados = new List<ChamadoModel>
            {
                new ChamadoModel("Erro 1", usuarioId, PrioridadeEnum.Alta),
                new ChamadoModel("Erro 2", usuarioId, PrioridadeEnum.Baixa)
            };

            _chamadoRepositoryMock.Setup(r => r.ObterPorUsuarioAsync(usuarioId))
                                  .ReturnsAsync(chamados);

            // Act
            var result = await _service.ListarChamadosPorUsuarioAsync(usuarioId);

            // Assert
            Assert.True(result.Status);
            Assert.NotNull(result.Dados);
            Assert.Equal(2, result.Dados!.Count);
            Assert.All(result.Dados, c => Assert.Equal(usuarioId, c.UsuarioId));
        }

        [Fact]
        public async Task ListarTodosOsChamadosAsync_DeveRetornarChamados()
        {
            // Arrange
            var chamados = new List<ChamadoModel>
            {
                new ChamadoModel("Erro 1", 1, PrioridadeEnum.Alta),
                new ChamadoModel("Erro 2", 2, PrioridadeEnum.Baixa)
            };

            _chamadoRepositoryMock.Setup(r => r.ObterTodosAsync())
                                  .ReturnsAsync(chamados);

            // Act
            var result = await _service.ListarTodosOsChamadosAsync();

            // Assert
            Assert.True(result.Status);
            Assert.NotNull(result.Dados);
            Assert.Equal(2, result.Dados!.Count);
        }
    }
}