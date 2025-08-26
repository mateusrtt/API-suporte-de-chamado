using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using api_sistema_de_chamado.Models;

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
                            throw new UnauthorizedAccessException("Token inválido ou não autorizado.");
                        },
                        OnForbidden = context =>
                        {
                            throw new UnauthorizedAccessException("Você não tem permissão para acessar esta funcionalidade.");
                        },
                        OnAuthenticationFailed = context =>
                        {
                            throw new ApplicationException("Falha ao processar o token JWT.", context.Exception);
                        }
                    };
                });
            return services;
        }
    }
}