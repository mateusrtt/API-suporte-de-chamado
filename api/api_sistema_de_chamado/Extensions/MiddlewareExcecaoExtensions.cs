using Api.Middlewares;

namespace Api.Extensions
{
    // Middleware global para tratamento de exceções
    public static class MiddlewareExcecaoExtensions
    {
        public static IApplicationBuilder UsarMiddlewareDeExcecao(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MiddlewareExcecao>();
        }
    }
}