namespace Api.Extensions
{
    public static class CorsExtensions
    {
        
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