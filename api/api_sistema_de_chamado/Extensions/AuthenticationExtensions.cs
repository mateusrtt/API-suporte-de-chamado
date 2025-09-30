using Domain.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.Extensions
{
    /// <summary>
    /// Extensão para configuração de autenticação JWT na aplicação.
    /// </summary>
    /// <remarks>
    /// - Valida a chave de assinatura do token (IssuerSigningKey) usando a chave definida em "AppSettings:Token".
    /// - Não valida issuer ou audience, considerando a aplicação interna.
    /// - Lança exceções personalizadas para cada evento:
    ///   - NaoAutorizadoException ao desafiar (token inválido ou ausente).
    ///   - ProibidoException quando o usuário não tem permissão.
    ///   - ServicoIndisponivelException em caso de falha ao processar o token.
    /// Deve ser chamada em Program para configurar a autenticação JWT.
    /// </remarks>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Adiciona autenticação JWT personalizada ao pipeline de serviços.
        /// </summary>
        /// <param name="services">O IServiceCollection da aplicação onde será registrada a autenticação.</param>
        /// <param name="configuration">O IConfiguration contendo as configurações, incluindo a chave do token JWT.</param>
        /// <returns>IServiceCollection com a autenticação JWT configurada, permitindo encadeamento de chamadas.</returns>
        public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!)
                        ),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            throw new NaoAutorizadoException("Token inválido ou não autorizado.");
                        },
                        OnForbidden = context =>
                        {
                            throw new ProibidoException("Você não tem permissão para acessar esta funcionalidade.");
                        },
                        OnAuthenticationFailed = context =>
                        {
                            throw new ServicoIndisponivelException("Falha ao processar o token JWT.");
                        }
                    };
                });
            return services;
        }
    }
}