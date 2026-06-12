using FinanceTrackerCM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerCM.Application.Interfaces;

public interface IAppDbContext
{
    // Definição dos DbSets para as entidades do domínio, que representam as tabelas do banco de dados
    public DbSet<Conta> Contas { get; set; } // DbSet para a entidade Conta, que representa as contas financeiras do usuário
    public DbSet<Categoria> Categorias { get; set; } // DbSet para a entidade Categoria, que representa as categorias de transações financeiras do usuário
    public DbSet<Transacao> Transacoes { get; set; } // DbSet para a entidade Transacao, que representa as transações financeiras do usuário
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); // Método para salvar as alterações no banco de dados de forma assíncrona, garantindo que as operações de criação, atualização e exclusão sejam persistidas no banco de dados

}
