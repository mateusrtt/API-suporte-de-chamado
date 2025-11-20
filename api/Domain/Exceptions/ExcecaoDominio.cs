using System.Net;

namespace Domain.Exceptions
{
    // Exceção base para todas as exceções do domínio
    // Armazena o código HTTP para padronizar respostas na API
    public class ExcecaoDominio : ApplicationException
    {
        public HttpStatusCode StatusCode { get; }

        protected ExcecaoDominio(HttpStatusCode statusCode, string mensagem)
            : base(mensagem)
        {
            StatusCode = statusCode;
        }
    }

    // Erro de conflito (409) usado quando já existe um registro igual
    public class ConflitoException : ExcecaoDominio
    {
        public ConflitoException(string mensagem)
            : base(HttpStatusCode.Conflict, mensagem) { }
    }

    // Erro de não encontrado (404)
    public class NaoEncontradoException : ExcecaoDominio
    {
        public NaoEncontradoException(string mensagem)
            : base(HttpStatusCode.NotFound, mensagem) { }
    }

    // Erro de validação (422) dados inválidos enviados pelo usuário
    public class ValidacaoException : ExcecaoDominio
    {
        public ValidacaoException(string mensagem)
            : base(HttpStatusCode.UnprocessableEntity, mensagem) { }
    }

    // Erro de não autorizado (401) usuário não autenticado
    public class NaoAutorizadoException : ExcecaoDominio
    {
        public NaoAutorizadoException(string mensagem)
            : base(HttpStatusCode.Unauthorized, mensagem) { }
    }

    // Erro de proibido (403) usuário autenticado, mas sem permissão
    public class ProibidoException : ExcecaoDominio
    {
        public ProibidoException(string mensagem)
            : base(HttpStatusCode.Forbidden, mensagem) { }
    }

    // Serviço indisponível (503) quando algum serviço falha ou está fora do ar
    public class ServicoIndisponivelException : ExcecaoDominio
    {
        public ServicoIndisponivelException(string mensagem)
            : base(HttpStatusCode.ServiceUnavailable, mensagem) { }
    }

    // Exceção usada para erros relacionados a JWT (token inválido, expirado, etc)
    public class ExcecaoJwt : ExcecaoDominio
    {
        public ExcecaoJwt(string mensagem)
            : base(HttpStatusCode.Unauthorized, mensagem) { }

        public ExcecaoJwt(HttpStatusCode statusCode, string mensagem)
            : base(statusCode, mensagem) { }
    }
}