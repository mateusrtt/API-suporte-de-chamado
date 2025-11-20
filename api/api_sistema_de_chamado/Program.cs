using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Api.Extensions;
using Api.Filters;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Application.Interface;
using Application.Services;
using Infrastructure.Data;
using Application.Configurations;
using Infrastructure.OpenIA;
using Infrastructure.OpenIA.Prompt;

var builder = WebApplication.CreateBuilder(args);

// Repositórios e serviços
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuario, UsuarioService>();
builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();
builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<ISenha, SenhaService>();
builder.Services.AddHttpClient<IOpenAi, OpenIAService>();
builder.Services.AddScoped<IPrompt, GeradorPromptPrioridade>();
builder.Services.AddScoped<IChamado, ChamadoService>();
builder.Services.AddScoped<IToken, TokenService>();
builder.Services.AddScoped<FiltroRespostaSucesso>();
builder.Services.AddCustomCors();
builder.Services.AddHttpContextAccessor();

// Configuração do JWT
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// Configuração do banco de dados
builder.Services.AddDbContext<AppDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configuração dos Controllers e filtros globais
builder.Services.AddCustomControllers(options =>
{
    options.Filters.Add<FiltroRespostaSucesso>();
});

// Desabilita o filtro padrão de ModelState
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Autenticação com JWT Bearer
builder.Services.AddCustomJwtAuthentication(builder.Configuration);

// Swagger 
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware global de exceção
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