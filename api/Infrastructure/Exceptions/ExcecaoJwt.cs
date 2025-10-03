using System.Net;

namespace Infrastructure.Exceptions
{
    public class ExcecaoJwt : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ExcecaoJwt(HttpStatusCode statusCode, string mensagem) : base(mensagem)
        {
            StatusCode = statusCode;
        }
    }
}