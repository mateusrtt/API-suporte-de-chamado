using Application.Interface;

namespace Infrastructure.OpenIA.Prompt
{
    public class GeradorPromptPrioridade : IPrompt
    {
        public string GerarPrompt(string descricao)
        {
         return
        "Você é um classificador automático de chamados de TI. " +
        "Sua tarefa é analisar a descrição e responder APENAS com um número (1, 2, 3 ou 4), seguindo as regras: \n\n" +

        "1 = Prioridade Baixa → Problemas simples ou solicitações sem urgência. " +
        "Exemplos: instalação de software, configuração de mouse, criação de usuário, dúvida de uso, pequenos ajustes.\n\n" +

        "2 = Prioridade Média → Problemas que afetam um usuário ou um setor, mas não param a empresa inteira. " +
        "Exemplos: impressora não funciona, acesso de um colaborador bloqueado, sistema lento para um departamento, falha em rede local.\n\n" +

        "3 = Prioridade Alta → Incidentes críticos que impactam várias pessoas ou sistemas essenciais da empresa. " +
        "Exemplos: sistema corporativo fora do ar, servidor inoperante, falha geral de e-mail, queda de rede da empresa, indisponibilidade de ERP.\n\n" +

        "4 = Analisando → Descrições vagas, irrelevantes, não relacionadas a TI ou sem sentido. " +
        "Exemplos: '12345#$%', 'quando será a reunião?', 'o ar-condicionado quebrou'.\n\n" +

        "Exemplos práticos:\n" +
        "\"Não consigo acessar meu e-mail corporativo\" → 3\n" +
        "\"A impressora do setor de compras não imprime\" → 2\n" +
        "\"Preciso instalar o Power BI\" → 1\n" +
        "\"Quando será a reunião da equipe?\" → 4\n" +
        "\"O servidor do ERP está fora do ar\" → 3\n" +
        "\"Meu mouse parou de funcionar\" → 1\n" +
        "\"A rede do prédio inteiro caiu\" → 3\n" +
        "\"A internet da minha sala está lenta\" → 2\n" +
        "\"Preciso de acesso ao sistema de ponto\" → 1\n\n" +
        "\"askjfdhkjsfksafhsajgfaskj\" → 4\n" +
        "\"Meu pagamento não caiu?\" → 4\n" +

         $"Descrição do chamado: \"{descricao}\"\n" +
         "Responda sempre apenas com um único número (1, 2, 3 ou 4), sem texto adicional e sem explicações.";
        }
    }
}