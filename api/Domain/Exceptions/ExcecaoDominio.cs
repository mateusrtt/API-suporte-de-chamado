using System.Net;

namespace Domain.Exceptions
{
    public class ExcecaoDominio : ApplicationException
    {
        public HttpStatusCode StatusCode { get; }

        protected ExcecaoDominio(HttpStatusCode statusCode, string mensagem)
            : base(mensagem)
        {
            StatusCode = statusCode;
        }
    }
    public class ConflitoException : ExcecaoDominio
    {
        public ConflitoException(string mensagem)
            : base(HttpStatusCode.Conflict, mensagem) { }
    }

    public class NaoEncontradoException : ExcecaoDominio
    {
        public NaoEncontradoException(string mensagem)
            : base(HttpStatusCode.NotFound, mensagem) { }
    }

    public class ValidacaoException : ExcecaoDominio
    {
        public ValidacaoException(string mensagem)
            : base(HttpStatusCode.UnprocessableEntity, mensagem) { }
    }

    public class NaoAutorizadoException : ExcecaoDominio
    {
        public NaoAutorizadoException(string mensagem)
            : base(HttpStatusCode.Unauthorized, mensagem) { }
    }

    public class ProibidoException : ExcecaoDominio
    {
        public ProibidoException(string mensagem)
            : base(HttpStatusCode.Forbidden, mensagem) { }
    }

    public class ServicoIndisponivelException : ExcecaoDominio
    {
        public ServicoIndisponivelException(string mensagem)
            : base(HttpStatusCode.ServiceUnavailable, mensagem) { }
    }
    public class ExcecaoJwt : ExcecaoDominio
    {
        public ExcecaoJwt(string mensagem)
            : base(HttpStatusCode.Unauthorized, mensagem) { }

        public ExcecaoJwt(HttpStatusCode statusCode, string mensagem)
            : base(statusCode, mensagem) { }
    }
}