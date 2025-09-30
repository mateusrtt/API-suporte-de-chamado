using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Extensions
{
    /// <summary>
    /// Extensão para configuração dos controllers da aplicação com opções personalizadas.
    /// </summary>
    /// <remarks>
    /// - Adiciona suporte a conversão de enums para string usando JsonStringEnumConverter.
    /// - Desativa políticas de nomenclatura para propriedades e chaves de dicionário, mantendo nomes originais.
    /// - Permite passar uma ação opcional de configuração do MvcOptions para customizar o comportamento dos controllers.
    /// Deve ser chamada em Program para configurar os controllers da aplicação.
    /// </remarks>
    public static class ControllersExtensions
    {
        /// <summary>
        /// Adiciona controllers com configuração JSON personalizada e opções de Mvc.
        /// </summary>
        /// <param name="services">O IServiceCollection da aplicação onde os controllers serão registrados.</param>
        /// <param name="configureMvc">Ação opcional para configurar MvcOptions, podendo alterar filtros, formatação e outros comportamentos.</param>
        /// <returns>IServiceCollection com os controllers configurados, permitindo encadeamento de chamadas.</returns>
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
