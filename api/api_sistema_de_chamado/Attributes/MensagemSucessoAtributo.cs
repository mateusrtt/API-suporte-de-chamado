namespace Api.Attributes
{
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