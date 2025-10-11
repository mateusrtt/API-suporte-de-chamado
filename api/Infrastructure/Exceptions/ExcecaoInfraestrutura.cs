using System.Net;

namespace Infrastructure.Exceptions
{
    public class ExcecaoInfraestrutura : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ExcecaoInfraestrutura(HttpStatusCode statusCode, string mensagem) : base(mensagem)
        {
            StatusCode = statusCode;
        }
    }
    public class PersistenciaException : ExcecaoInfraestrutura
    {
        public PersistenciaException(string mensagem)
            : base(HttpStatusCode.InternalServerError, mensagem) { }
    }

    public class BancoDeDadosException : ExcecaoInfraestrutura
    {
        public BancoDeDadosException(string mensagem)
            : base(HttpStatusCode.ServiceUnavailable, mensagem) { }
    }

    public class IntegridadeException : ExcecaoInfraestrutura
    {
        public IntegridadeException(string mensagem)
            : base(HttpStatusCode.Conflict, mensagem) { }
    }
}