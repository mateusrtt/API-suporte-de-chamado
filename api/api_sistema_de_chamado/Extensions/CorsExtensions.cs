namespace Api.Extensions
{
    public static class CorsExtensions
    {
        // Configura política de CORS
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()   
                        .AllowAnyHeader()   
                        .AllowAnyMethod();
                });
            });
            return services;
        }
    }
}