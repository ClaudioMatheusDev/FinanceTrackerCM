using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerCM.Context.AppDbContext
{   

    public class AppDbContext : DbContext
    {
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Conta> Contas { get; set; }
        
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }


}