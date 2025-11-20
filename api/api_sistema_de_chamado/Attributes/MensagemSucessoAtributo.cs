namespace Api.Attributes
{
    // Atributo usado para definir uma mensagem de sucesso personalizada em métodos da API

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class MensagemSucessoAtributo : Attribute
    {
        public string Mensagem { get; }

        public MensagemSucessoAtributo(string mensagem)
        {
            Mensagem = mensagem;
        }
    }
}