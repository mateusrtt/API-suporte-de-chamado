
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace api_sistema_de_chamado.Services.OpenAiService
{
    public class OpenAiService : IOpenAiInterface
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
                throw new Exception("Chave da API OpenAI não configurada.");

            var prompt = $"Classifique a prioridade deste chamado como 1 (Baixa), 2 (Média) ou 3 (Alta) baseada na descrição: \"{descricao}\". Responda apenas com o número sem texto extra ou explicações.";

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

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro da OpenAI: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var resposta = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            var respostaLimpa = new string(resposta?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());

            if (int.TryParse(respostaLimpa, out int prioridade) && prioridade >= 1 && prioridade <= 3)
                return prioridade;

            throw new Exception("Resposta inválida da IA.");
        }
    }
}
