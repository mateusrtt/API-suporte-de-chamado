namespace api_sistema_de_chamado.Services.OpenAiService
{
    public interface IOpenAiInterface
    {
        Task<int> ObterPrioridadePorDescricaoAsync(string descricao);
    }
}
