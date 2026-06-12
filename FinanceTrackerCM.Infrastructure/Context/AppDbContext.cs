using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Domain.Entities;
using FinanceTrackerCM.Application.Interfaces;
using AuditLogCM.EFCore.Interceptors;

namespace FinanceTrackerCM.Infrastructure.Context
{
    // DbContext da aplicação, responsável por gerenciar as entidades do domínio e as operações de banco de dados
    public class AppDbContext : DbContext, IAppDbContext
    {
        // Campo para armazenar o interceptor de auditoria, que será usado para registrar as operações de banco de dados
        private readonly AuditInterceptor _auditInterceptor;

        // Definição dos DbSets para as entidades do domínio
        public DbSet<Transacao> Transacoes { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Conta> Contas { get; set; } = null!;

        // Construtor do DbContext que recebe as opções de configuração e o interceptor de auditoria
        public AppDbContext(// Parâmetros do construtor: opções de configuração do DbContext e interceptor de auditoria
            DbContextOptions<AppDbContext> options,
            AuditInterceptor auditInterceptor) : base(options)
        {       // Atribuição do interceptor de auditoria para uso nas operações de banco de dados
            _auditInterceptor = auditInterceptor;
        }
        // Configuração do interceptor de auditoria para registrar as operações de banco de dados
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { // Adição do interceptor de auditoria às opções de configuração do DbContext
            optionsBuilder.AddInterceptors(_auditInterceptor);
        }
    }
}