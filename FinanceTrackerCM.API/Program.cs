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



DotNetEnv.Env.Load(); // Carrega as variáveis de ambiente do arquivo .env para que possam ser acessadas durante a execução da aplicação, permitindo que as configurações sensíveis, como chaves de API e strings de conexão, sejam mantidas fora do código-fonte e facilmente configuráveis em diferentes ambientes (desenvolvimento, produção, etc.)
//var key = Environment.GetEnvironmentVariable("JWT_KEY"); // Obtém a chave JWT das variáveis de ambiente, que é usada para assinar os tokens de acesso e garantir a segurança da autenticação na aplicação. Essa chave deve ser mantida em segredo e configurada corretamente para evitar vulnerabilidades de segurança relacionadas à autenticação e autorização dos usuários na aplicação.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();


builder.Services.AddHttpContextAccessor();

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configuração da autenticação JWT
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY"); // Obtém a chave JWT das variáveis de ambiente, que é usada para assinar os tokens de acesso e garantir a segurança da autenticação na aplicação. Essa chave deve ser mantida em segredo e configurada corretamente para evitar vulnerabilidades de segurança relacionadas à autenticação e autorização dos usuários na aplicação.
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();