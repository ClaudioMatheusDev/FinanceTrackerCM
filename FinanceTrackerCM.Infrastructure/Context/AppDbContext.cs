using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Domain.Entities;
using AuditLogCM.EFCore.Interceptors;

namespace FinanceTrackerCM.Infrastructure.Context
{   

    public class AppDbContext : DbContext
    {

        private readonly AuditInterceptor _auditInterceptor;


        public DbSet<Transacao> Transacoes { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Conta> Contas { get; set; } = null!;


        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            AuditInterceptor auditInterceptor) : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditInterceptor);
        }
    }


}