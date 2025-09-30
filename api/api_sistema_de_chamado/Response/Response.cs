namespace Api.Response
{
    /// <summary>
    /// Modelo padrão para respostas da API.
    /// </summary>
    /// <typeparam name="T">Tipo dos dados retornados.</typeparam>
    public class Response<T>
    {
        /// <summary>
        /// Dados da resposta.
        /// </summary>
        public T? Dados { get; set; }

        /// <summary>
        /// Mensagem de sucesso ou erro.
        /// </summary>
        public string? Mensagem { get; set; } = string.Empty;

        /// <summary>
        /// Indica se a operação foi bem-sucedida.
        /// </summary>
        public bool Status { get; set; } = true;
    }
}