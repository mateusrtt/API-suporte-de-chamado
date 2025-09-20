using System.Net;

namespace Domain.Exceptions
{
    public class ExcecaoAplicacao : ApplicationException
    {
        public HttpStatusCode StatusCode { get; }

        protected ExcecaoAplicacao(HttpStatusCode statusCode, string mensagem)
            : base(mensagem)
        {
            StatusCode = statusCode;
        }
    }
    public class ConflitoException : ExcecaoAplicacao
    {
        public ConflitoException(string mensagem)
            : base(HttpStatusCode.Conflict, mensagem) { }
    }

    public class NaoEncontradoException : ExcecaoAplicacao
    {
        public NaoEncontradoException(string mensagem)
            : base(HttpStatusCode.NotFound, mensagem) { }
    }

    public class ValidacaoException : ExcecaoAplicacao
    {
        public ValidacaoException(string mensagem)
            : base(HttpStatusCode.UnprocessableEntity, mensagem) { }
    }

    public class NaoAutorizadoException : ExcecaoAplicacao
    {
        public NaoAutorizadoException(string mensagem)
            : base(HttpStatusCode.Unauthorized, mensagem) { }
    }

    public class ProibidoException : ExcecaoAplicacao
    {
        public ProibidoException(string mensagem)
            : base(HttpStatusCode.Forbidden, mensagem) { }
    }

    public class ServicoIndisponivelException : ExcecaoAplicacao
    {
        public ServicoIndisponivelException(string mensagem)
            : base(HttpStatusCode.ServiceUnavailable, mensagem) { }
    }
}