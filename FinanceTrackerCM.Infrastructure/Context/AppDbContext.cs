using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AuditLogCM.EFCore.Interceptors;
using FinanceTrackerCM.Domain.Entities;
using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Application.Users;

namespace FinanceTrackerCM.Infrastructure.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IAppDbContext
{
    private readonly AuditInterceptor _auditInterceptor;
    private readonly FinanceTrackerCM.Application.Interfaces.ICurrentUserResolver _currentUser;

    public DbSet<Transacao> Transacoes { get; set; } = null!;
    public DbSet<Categoria> Categorias { get; set; } = null!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<Conta> Contas { get; set; } = null!;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        AuditInterceptor auditInterceptor,
        ICurrentUserResolver currentUser) : base(options)
    {
        _auditInterceptor = auditInterceptor;
        _currentUser = currentUser;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Exemplo de query filter usando TenantId (assume propriedades TenantId nas entidades)
        modelBuilder.Entity<Conta>().HasQueryFilter(c => c.TenantId == _currentUser.TenantId);
        modelBuilder.Entity<Categoria>().HasQueryFilter(c => c.TenantId == _currentUser.TenantId);
        modelBuilder.Entity<Transacao>().HasQueryFilter(t => t.TenantId == _currentUser.TenantId);
    }
}