using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Api.Extensions;
using Api.Filters;
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
builder.Services.AddScoped<IUsuario, UsuarioService>();
builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();
builder.Services.AddScoped<IAcessoLogRepository, AcessoLogRepository>();
builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<ISenha, SenhaService>();
builder.Services.AddHttpClient<IOpenAi, OpenAiService>();
builder.Services.AddScoped<IChamado, ChamadoService>();
builder.Services.AddScoped<FiltroLogAcesso>();
builder.Services.AddScoped<FiltroRespostaSucesso>();
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
    options.Filters.Add<FiltroLogAcesso>();
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

app.UsarMiddlewareDeExcecao();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();