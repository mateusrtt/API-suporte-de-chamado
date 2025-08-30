using api_sistema_de_chamado.Data;
using api_sistema_de_chamado.Repositories.Chamado;
using api_sistema_de_chamado.Repositories.Usuario;
using api_sistema_de_chamado.Services.AuthService;
using api_sistema_de_chamado.Services.ChamadoService;
using api_sistema_de_chamado.Services.OpenAiService;
using api_sistema_de_chamado.Services.SenhaService;
using Microsoft.EntityFrameworkCore;
using api_sistema_de_chamado.Extensions;
using api_sistema_de_chamado.Middlewares;
using api_sistema_de_chamado.Filters;
using api_sistema_de_chamado.Repositories.AcessoLog;

var builder = WebApplication.CreateBuilder(args);

// Configura Controllers e enums como string
builder.Services.AddCustomControllers();
builder.Services.AddEndpointsApiExplorer();

// Repositórios e serviços
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IChamadoRepository, ChamadoRepository>();
builder.Services.AddScoped<IAcessoLogRepository, AcessoLogRepository>();
builder.Services.AddScoped<IAuthInterface, AuthService>();
builder.Services.AddScoped<ISenhaInterface, SenhaService>();
builder.Services.AddHttpClient<IOpenAiInterface, OpenAiService>();
builder.Services.AddScoped<IChamadoInterface, ChamadoService>();
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
app.UseLoginLogs();
app.MapControllers();
app.Run();