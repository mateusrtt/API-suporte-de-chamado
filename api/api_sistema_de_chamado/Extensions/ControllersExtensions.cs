using System.Text.Json.Serialization;

namespace API.Extensions
{
    public static class ControllersExtensions
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            return services;
        }
    }
}
