using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Domain.Enums;
using Application.Interface;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class OpenAiService : IOpenAi
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<int> ObterPrioridadePorDescricaoAsync(string descricao)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                return (int)PrioridadeEnum.Analisando;

            var prompt = $"Classifique a prioridade deste chamado como 1 (Baixa), 2 (Média) ou 3 (Alta) baseada na descrição: \"{descricao}\". " +
                $"Se a descrição for confusa, sem sentido, números aleatórios, caracteres aleatórios ou não puder ser classificada com confiança, responda 4 (Analisando). " +
                $"Responda sempre apenas com o número sem texto extra ou explicações.";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
               {
                    new { role = "user", content = prompt }
                },
                max_tokens = 5,
                temperature = 0
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            try
            {
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
                    return (int)PrioridadeEnum.Analisando;

                var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var resposta = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            var respostaLimpa = new string(resposta?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());

             if (int.TryParse(respostaLimpa, out int prioridade) && Enum.IsDefined(typeof(PrioridadeEnum), prioridade))
                 return prioridade;

                return (int)PrioridadeEnum.Analisando;
            }
            catch 
            {
                return (int)PrioridadeEnum.Analisando;
            }
        }
    }
}