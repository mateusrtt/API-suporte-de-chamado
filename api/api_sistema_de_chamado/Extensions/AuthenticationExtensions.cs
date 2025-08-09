using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace api_sistema_de_chamado.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!)),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var json = """
                            {
                                "status": false,
                                "mensagem": "Token inválido ou não autorizado.",
                                "dados": null
                            }
                            """;
                            return context.Response.WriteAsync(json);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var json = """
                            {
                                "status": false,
                                "mensagem": "Você não tem permissão para acessar esta funcionalidade.",
                                "dados": null
                            }
                            """;
                            return context.Response.WriteAsync(json);
                        }
                    };
                });
            return services;
        }
    }
}
