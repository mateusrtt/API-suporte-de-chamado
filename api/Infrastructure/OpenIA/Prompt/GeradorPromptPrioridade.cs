using Application.Interface;

namespace Infrastructure.OpenIA.Prompt
{
    public class GeradorPromptPrioridade : IPrompt
    {
        public string GerarPrompt(string descricao)
        {
            return
                $"Você é um classificador de chamados de TI. Sua tarefa é analisar a descrição de um chamado e responder apenas com um número que represente a prioridade, com base na descrição: \"{descricao}\". " +
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
        }
    }
}