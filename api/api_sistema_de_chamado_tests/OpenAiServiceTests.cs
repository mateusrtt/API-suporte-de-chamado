using Application.Interface;
using Domain.Enums;
using Infrastructure.OpenIA;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;


namespace Api_tests
{
    public class OpenAiServiceTests
    {
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

        private IPrompt CriarPromptMock()
        {
            var promptMock = new Mock<IPrompt>();
            promptMock.Setup(p => p.GerarPrompt(It.IsAny<string>()))
                      .Returns("prompt fake para testes");
            return promptMock.Object;
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

            var service = new OpenIAService(httpClient, configMock.Object, CriarPromptMock());

            // Act
            int prioridade = await service.ObterPrioridadePorDescricao(descricao);

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

            var service = new OpenIAService(httpClient, configMock.Object, CriarPromptMock());

            // Act
            int prioridade = await service.ObterPrioridadePorDescricao("Teste");

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

            var service = new OpenIAService(httpClient, configMock.Object, CriarPromptMock());

            // Act
            int prioridade = await service.ObterPrioridadePorDescricao("Teste");

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

            var service = new OpenIAService(httpClient, configMock.Object, CriarPromptMock());

            // Act
            int prioridade = await service.ObterPrioridadePorDescricao("Teste");

            // Assert
            Assert.Equal((int)PrioridadeEnum.Analisando, prioridade);
        }
    }
}