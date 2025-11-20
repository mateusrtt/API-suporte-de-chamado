namespace Api.Response
{
    // Classe genérica usada como formato padrão de resposta da API
    public class Response<T>
    {
        public T? Dados { get; set; }

        public string? Mensagem { get; set; } = string.Empty;

        public bool Status { get; set; } = true;
    }
}