using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Infrastructure.Context;
using FinanceTrackerCM.Infrastructure.Services;
using AuditLogCM.Core.Interfaces;
using AuditLogCM.EFCore.Interceptors;
using AuditLogCM.EFCore.DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

// Injeção de dependência para resolver o usuário atual
builder.Services.AddScoped<ICurrentUserResolver, CurrentUserResolver>();
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();