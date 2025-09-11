using Microsoft.EntityFrameworkCore;
using api_sistema_de_chamado.Middlewares;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using API.Filters;
using API.Middlewares;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Application.Interface;
using Application.Services;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura Controllers e enums como string
builder.Services.AddCustomControllers();
builder.Services.AddEndpointsApiExplorer();

// Repositórios e serviços
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();
builder.Services.AddScoped<IAcessoLogRepository, AcessoLogRepository>();
builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<ISenha, SenhaService>();
builder.Services.AddHttpClient<IOpenAi, OpenAiService>();
builder.Services.AddScoped<IChamado, ChamadoService>();
builder.Services.AddScoped<IAcessoLog, AcessoLogService>();
builder.Services.AddCustomCors();
builder.Services.AddHttpContextAccessor();

// Banco de dados
builder.Services.AddDbContext<AppDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Filtro de resposta de sucesso
builder.Services.AddControllers(options =>
{
    options.Filters.Add<FiltroRespostaSucesso>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Autenticação JWT
builder.Services.AddCustomJwtAuthentication(builder.Configuration);

// Swagger 
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseGlobalExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAcessoLogs();
app.MapControllers();
app.Run();