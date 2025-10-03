using Application.Interface;
using Domain.Enums;
using Infrastructure.OpenIA.Response;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace Infrastructure.OpenIA
{
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

        public async Task<int> ObterPrioridadePorDescricao(string descricao)
        {
            var prompt = _prompt.GerarPrompt(descricao);

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
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (!response.IsSuccessStatusCode)
                    return (int)PrioridadeEnum.Analisando;

                var json = await response.Content.ReadAsStringAsync();
                var completion = JsonSerializer.Deserialize<ChatCompletionResponse>(
                    json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                var resposta = completion?.Choices?.FirstOrDefault()?.Message?.Content;
                var respostaLimpa = new string(resposta?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());

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