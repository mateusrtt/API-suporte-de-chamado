using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Extensions
{
    public static class ControllersExtensions
    {
        // Configura os controllers e ajusta as opções de serialização JSON
        public static IServiceCollection AddCustomControllers(this IServiceCollection services, Action<MvcOptions>? configureMvc = null)
        {
            services.AddControllers(options =>
            {
                configureMvc?.Invoke(options);
            })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });
            return services;
        }
    }
}
