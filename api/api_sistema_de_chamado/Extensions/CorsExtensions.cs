namespace Api.Extensions
{
    /// <summary>
    /// Extensão para configurar CORS na aplicação.
    /// Permite definir origens permitidas, headers e métodos HTTP.
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// Adiciona a política de CORS personalizada chamada "CorsPolicy".
        /// </summary>
        /// <param name="services">IServiceCollection da aplicação para registrar a política.</param>
        /// <returns>IServiceCollection com a política de CORS registrada.</returns>
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            var allowedOrigins = new string[]
            {
            "http://127.0.0.1:5500",
            };

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
