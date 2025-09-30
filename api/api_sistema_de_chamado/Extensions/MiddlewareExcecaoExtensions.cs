using Api.Middlewares;

namespace Api.Extensions
{
    /// <summary>
    /// Adiciona o middleware de exceções à pipeline da aplicação.
    /// </summary>
    public static class MiddlewareExcecaoExtensions
    {
        /// <summary>
        /// Registra o MiddlewareExcecao no IApplicationBuilder.
        /// </summary>
        public static IApplicationBuilder UsarMiddlewareDeExcecao(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MiddlewareExcecao>();
        }
    }
}