namespace Api.Attributes
{
    /// <summary>
    /// Atributo para definir uma mensagem de sucesso personalizada para métodos de ação.
    /// </summary>
    /// <remarks>
    /// Pode ser aplicado apenas em métodos de controller.
    /// Usado em conjunto com o FiltroRespostaSucesso, 
    /// para padronizar e exibir mensagens de sucesso na API.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MensagemSucessoAtributo : Attribute
    {
        /// <summary>
        /// Mensagem de sucesso que será exibida na resposta da API.
        /// </summary>
        public string Mensagem { get; }

        /// <summary>
        /// Construtor que recebe a mensagem de sucesso personalizada.
        /// </summary>
        /// <param name="mensagem">Texto da mensagem de sucesso.</param>
        public MensagemSucessoAtributo(string mensagem)
        {
            Mensagem = mensagem;
        }
    }
}