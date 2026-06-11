using FinanceTrackerCM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerCM.Application.Interfaces;

    public interface IAppDbContext
    {
        
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
