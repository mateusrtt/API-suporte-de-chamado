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
                            return WriteErrorResponse(context.Response,
                                StatusCodes.Status401Unauthorized,
                                "Token inválido ou não autorizado.");
                        },
                        OnForbidden = context =>
                        {
                            return WriteErrorResponse(context.Response,
                                StatusCodes.Status403Forbidden,
                                "Você não tem permissão para acessar esta funcionalidade.");
                        },
                        OnAuthenticationFailed = context =>
                        {
                            return WriteErrorResponse(context.Response,
                                StatusCodes.Status400BadRequest,
                                "Falha ao processar o token JWT.",
                                context.Exception?.Message);
                        }
                    };
                });
            return services;
        }

        private static Task WriteErrorResponse(HttpResponse response, int statusCode, string mensagem, object? dados = null)
        {
            if (response.HasStarted)
                return Task.CompletedTask;
            
            response.StatusCode = statusCode;
            response.ContentType = "application/json";

            var result = new Response<object>
            {
                Status = false,
                Mensagem = mensagem,
                Dados = null
            };

            return response.WriteAsync(JsonSerializer.Serialize(result));
        }
    }
}