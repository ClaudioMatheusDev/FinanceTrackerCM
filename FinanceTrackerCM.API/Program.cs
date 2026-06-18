using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Infrastructure.Services;
using AuditLogCM.Core.Interfaces;
using AuditLogCM.EFCore.Interceptors;
using AuditLogCM.EFCore.DbContext;
using Microsoft.EntityFrameworkCore;
using AuditLogCM.EFCore.Serializers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinanceTrackerCM.Application.Users;
using Microsoft.AspNetCore.Identity;
using FinanceTrackerCM.Infrastructure.Context;
using FluentValidation;



DotNetEnv.Env.Load(); // Carrega as variáveis de ambiente do arquivo .env para que possam ser acessadas durante a execução da aplicação, permitindo que as configurações sensíveis, como chaves de API e strings de conexão, sejam mantidas fora do código-fonte e facilmente configuráveis em diferentes ambientes (desenvolvimento, produção, etc.)
//var key = Environment.GetEnvironmentVariable("JWT_KEY"); // Obtém a chave JWT das variáveis de ambiente, que é usada para assinar os tokens de acesso e garantir a segurança da autenticação na aplicação. Essa chave deve ser mantida em segredo e configurada corretamente para evitar vulnerabilidades de segurança relacionadas à autenticação e autorização dos usuários na aplicação.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();


builder.Services.AddHttpContextAccessor();

// CORS para o frontend local (Vite)
var viteOrigin = builder.Configuration["Vite:Url"] ?? "http://localhost:5174";
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(viteOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configuração da autenticação JWT
// Preferir `Jwt:Key` na configuração; fallback para `JWT_KEY` env var se necessário
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey))
    throw new InvalidOperationException("Configuração 'Jwt:Key' não encontrada ou vazia.");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))  
    };
});

// Injeção de dependência para resolver o usuário atual
builder.Services.AddScoped<FinanceTrackerCM.Application.Interfaces.ICurrentUserResolver, CurrentUserResolver>();
builder.Services.AddScoped<AuditLogCM.Core.Interfaces.ICurrentUserResolver, CurrentUserResolver>();
// Configuração do serviço de auditoria
builder.Services.AddScoped<IAuditSerializer, JsonAuditSerializer>();
builder.Services.AddScoped<AuditInterceptor>();
// Configuração do DbContext com o interceptor de auditoria
builder.Services.AddDbContext<AuditDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configuração do DbContext da aplicação
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Registro do DbContext da aplicação para injeção de dependência
builder.Services.AddScoped<IAppDbContext, AppDbContext>();
// Configuração do MediatR para registrar os handlers
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(FinanceTrackerCM.Application.UseCases.Contas.CriarContaCommand).Assembly));

builder.Services.AddScoped<ITokenService, TokenService>(); // Registro do serviço de token para injeção de dependência, permitindo que a aplicação utilize a implementação concreta do TokenService para criar tokens de acesso e atualização durante os processos de autenticação e autorização dos usuários na aplicação
// Registro do serviço de relatórios (PDF/Excel)
builder.Services.AddScoped<IReportService, ReportService>();

// Registrando serviços finais antes de construir a aplicação
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features
            .Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?
            .Error;

        var (statusCode, message) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage))),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                exception.Message),
            InvalidOperationException => (
                StatusCodes.Status404NotFound,
                exception.Message),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Ocorreu um erro inesperado.")
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new { message });
    });
});

app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
