using Application.Configurations;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddCustomJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = new JwtConfig();
            configuration.GetSection("JwtConfig").Bind(jwtConfig);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtConfig.Token)
                        ),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            throw new ExcecaoJwt("Token inválido ou não autorizado.");
                        },
                        OnForbidden = context =>
                        {
                            throw new ExcecaoJwt(HttpStatusCode.Forbidden, "Você não tem permissão para acessar esta funcionalidade.");
                        },
                        OnAuthenticationFailed = context =>
                        {
                            throw new ExcecaoJwt(HttpStatusCode.ServiceUnavailable, "Falha ao processar o token JWT.");
                        }
                    };
                });
            return services;
        }
    }
}