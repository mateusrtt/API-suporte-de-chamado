using api_sistema_de_chamado.Services.OpenAiService;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace api_sistema_de_chamado_tests
{
    public class OpenAiServiceTests
    {
        // Classe interna usada para simular o comportamento do HttpClient
        private class HttpMessageHandlerMock : HttpMessageHandler
        {
            private readonly HttpResponseMessage _fakeResponse;

            public HttpMessageHandlerMock(HttpResponseMessage fakeResponse)
            {
                _fakeResponse = fakeResponse;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(_fakeResponse);
            }
        }

        [Theory]
        [InlineData("Meu computador não liga e preciso de ajuda urgente.", 2)]
        [InlineData("A tela do monitor está piscando.", 1)]
        [InlineData("Solicito troca de teclado.", 3)] 
        public async Task ObterPrioridadePorDescricaoAsync_DeveRetornarPrioridadeEsperada(string descricao, int prioridadeEsperada)
        {
            // Arrange
            var fakeJson = $@"
            {{
                ""choices"": [
                    {{
                        ""message"": {{
                            ""content"": ""{prioridadeEsperada}""
                        }}
                    }}
                ]
            }}";

            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(fakeJson, Encoding.UTF8, "application/json")
            };

            var handlerMock = new HttpMessageHandlerMock(fakeResponse);
            var httpClient = new HttpClient(handlerMock);

            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["OpenAI:ApiKey"]).Returns("fake-api-key");

            var service = new OpenAiService(httpClient, configMock.Object);

            // Act
            int prioridade = await service.ObterPrioridadePorDescricaoAsync(descricao);

            // Assert
            Assert.Equal(prioridadeEsperada, prioridade);
        }

        [Fact]
        public async Task ObterPrioridadePorDescricaoAsync_DeveLancarErroDeConexao_SemInternet()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException());

            var httpClient = new HttpClient(handlerMock.Object);
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["OpenAI:ApiKey"]).Returns("fake-api-key");

            var service = new OpenAiService(httpClient, configMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                service.ObterPrioridadePorDescricaoAsync("Teste"));

            Assert.Equal("Sem conexão com a internet. Verifique sua conexão para enviar o chamado.", ex.Message);
        }

        [Fact]
        public async Task ObterPrioridadePorDescricaoAsync_DeveLancarErroDeTimeout_QuandoARequisicaoExpira()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException());

            var httpClient = new HttpClient(handlerMock.Object);
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["OpenAI:ApiKey"]).Returns("fake-api-key");

            var service = new OpenAiService(httpClient, configMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                service.ObterPrioridadePorDescricaoAsync("Teste"));

            Assert.Equal("A requisição expirou. Verifique sua conexão com a internet.", ex.Message);
        }

        [Fact]
        public async Task ObterPrioridadePorDescricaoAsync_DeveLancarErro_QuandoRespostaForInvalida()
        {
            // Arrange
            var fakeJson = @"
        {
            ""choices"": [
                    {
                    ""message"": {
                        ""content"": ""não entendi sua pergunta""
                    }
                }
            ]
        }";

            var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(fakeJson, Encoding.UTF8, "application/json")
            };

            var handlerMock = new HttpMessageHandlerMock(fakeResponse);
            var httpClient = new HttpClient(handlerMock);

            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["OpenAI:ApiKey"]).Returns("fake-api-key");

            var service = new OpenAiService(httpClient, configMock.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                service.ObterPrioridadePorDescricaoAsync("Teste"));

            Assert.Equal("Não foi possível classificar o chamado automaticamente. Tente novamente mais tarde.", ex.Message);
        }
    }
}
