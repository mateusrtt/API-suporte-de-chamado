using System.Net;

namespace Infrastructure.Exceptions
{
    // Exceção base da camada de infraestrutura
    public class ExcecaoInfraestrutura : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ExcecaoInfraestrutura(HttpStatusCode statusCode, string mensagem) : base(mensagem)
        {
            StatusCode = statusCode;
        }
    }

    // Erros relacionados a persistência de dados (salvar, atualizar, etc)
    public class PersistenciaException : ExcecaoInfraestrutura
    {
        public PersistenciaException(string mensagem)
            : base(HttpStatusCode.InternalServerError, mensagem) { }
    }

    // Erros gerais de banco de dados (conexão, indisponibilidade, timeouts)
    public class BancoDeDadosException : ExcecaoInfraestrutura
    {
        public BancoDeDadosException(string mensagem)
            : base(HttpStatusCode.ServiceUnavailable, mensagem) { }
    }

    // Erros de integridade (dados duplicados, violação de FK, etc.)
    public class IntegridadeException : ExcecaoInfraestrutura
    {
        public IntegridadeException(string mensagem)
            : base(HttpStatusCode.Conflict, mensagem) { }
    }
}