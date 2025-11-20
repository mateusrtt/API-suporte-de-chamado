using Application.Interface;
using Domain.Enums;
using Infrastructure.OpenIA.Response;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Infrastructure.OpenIA
{
    // Serviço responsável por enviar requisições à OpenAI para classificar a prioridade de um chamado
    public class OpenIAService : IOpenAi
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IPrompt _prompt;

        public OpenIAService(HttpClient httpClient, IConfiguration configuration, IPrompt prompt)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _prompt = prompt;

            var apiKey = _configuration["OpenAI:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);
            }
        }

        /// <summary>
        /// Envia a descrição do chamado para a OpenAI
        /// e retorna a prioridade (1 a 4).
        /// </summary>
        public async Task<int> ObterPrioridadePorDescricao(string descricao)
        {
            var prompt = _prompt.GerarPrompt(descricao);

            // Corpo da requisição usado pelo modelo da OpenAI
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] { new { role = "user", content = prompt } },
                max_tokens = 5,
                temperature = 0
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try
            {
                // Corpo da requisição usado pelo modelo da OpenAI
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                // Se falhar, retorna prioridade padrão (Analisando)
                if (!response.IsSuccessStatusCode)
                    return (int)PrioridadeEnum.Analisando;

                var json = await response.Content.ReadAsStringAsync();

                // Desserializa a resposta no modelo esperado
                var completion = JsonSerializer.Deserialize<ChatCompletionResponse>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                // Extrai somente números da resposta
                var resposta = completion?.Choices?.FirstOrDefault()?.Message?.Content;
                var respostaLimpa = new string(resposta?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());

                // Converte para inteiro e valida se é um enum válido
                if (int.TryParse(respostaLimpa, out int prioridade) &&
                    Enum.IsDefined(typeof(PrioridadeEnum), prioridade))
                {
                    return prioridade;
                }

                return (int)PrioridadeEnum.Analisando;
            }
            catch
            {
                return (int)PrioridadeEnum.Analisando;
            }
        }
    }
}