namespace Application.Interface
{
    public interface IAcessoLog
    {
        Task Registrar(int? usuarioId, string nome, string email, string ip, string metodo, string url);
    }
}