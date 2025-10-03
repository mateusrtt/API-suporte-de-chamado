using Api.Middlewares;

namespace Api.Extensions
{
    public static class MiddlewareExcecaoExtensions
    {
        public static IApplicationBuilder UsarMiddlewareDeExcecao(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MiddlewareExcecao>();
        }
    }
}