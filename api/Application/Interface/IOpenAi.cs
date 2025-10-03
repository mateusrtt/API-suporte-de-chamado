namespace Application.Interface
{
    public interface IOpenAi
    {
        Task<int> ObterPrioridadePorDescricao(string descricao);
    }
}