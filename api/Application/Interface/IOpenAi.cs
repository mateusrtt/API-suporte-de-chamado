namespace Application.Interface
{
    public interface IOpenAi
    {
        Task<int> ObterPrioridadePorDescricaoAsync(string descricao);
    }
}
