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

            var prompt = $"Você é um classificador de chamados de TI. Sua tarefa é analisar a descrição de um chamado e responder apenas com um número que represente a prioridade, com base na descrição: \"{descricao}\". " +
                         $"(1 = Prioridade Baixa, 2 = Prioridade Média, 3 = Prioridade Alta, 4 = Analisando) para chamados relacionados a TI, como sistemas, redes, computadores, senhas, software, e-mails, hardware ou assuntos semelhantes. " +
                         $"Se a descrição não estiver relacionada a TI, for ambígua, confusa, irrelevante ou contiver apenas números/caracteres aleatórios, responda com 4 (Analisando). " +
                         $"Sempre responda apenas com o número 1, 2, 3 ou 4, sem texto extra ou explicações. " +
                         $"Exemplos: " +
                         $"\"Não consigo acessar meu e-mail corporativo\" 3, " +
                         $"\"A impressora do departamento não está imprimindo\" 2, " +
                         $"\"Preciso instalar um novo software de planilhas\" 1, " +
                         $"\"Quando será a reunião de equipe?\" 4, " +
                         $"\"12345#$%\" 4, " +
                         $"\"O ar-condicionado está muito frio no escritório\" 4;";

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