using Application.Services;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;


namespace Api_tests
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
        public async Task ObterPrioridadePorDescricaoAsync_SemInternet_DeveRetornarAnalisando()
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

            // Act
            int prioridade = await service.ObterPrioridadePorDescricaoAsync("Teste");

            // Assert
            Assert.Equal((int)PrioridadeEnum.Analisando, prioridade);
        }

        [Fact]
        public async Task ObterPrioridadePorDescricaoAsync_Timeout_DeveRetornarAnalisando()
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

            // Act
            int prioridade = await service.ObterPrioridadePorDescricaoAsync("Teste");

            // Assert
            Assert.Equal((int)PrioridadeEnum.Analisando, prioridade);
        }

        [Fact]
        public async Task ObterPrioridadePorDescricaoAsync_RespostaInvalida_DeveRetornarAnalisando()
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

            // Act
            int prioridade = await service.ObterPrioridadePorDescricaoAsync("Teste");

            // Assert
            Assert.Equal((int)PrioridadeEnum.Analisando, prioridade);
        }
    }
}