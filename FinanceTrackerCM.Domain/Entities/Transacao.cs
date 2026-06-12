using FinanceTrackerCM.Domain.Enums;

namespace FinanceTrackerCM.Domain.Entities
{    // Entidade que representa uma categoria de transação financeira do usuário
     // Exemplo de categorias:

    public class Transacao
    {   // Propriedades da entidade Transacao, que representam os dados de uma transação financeira do usuário
        public Guid Id { get; set; } // Identificador único da transação
        public Guid IdConta { get; set; } // Identificador da conta associada à transação
        public Guid IdCategoria { get; set; } // Identificador da categoria associada à transação
        public Guid IdUsuario { get; set; } // Identificador do usuário proprietário da transação
        public TipoTransacao Tipo { get; set; } // Tipo da transação (Receita ou Despesa)
        public StatusTransacao Status { get; set; }// Status da transação (Pendente, Concluída, Cancelada)
        public Conta Conta { get; set; } = null!; // Conta associada à transação
        public Categoria Categoria { get; set; } = null!; // Categoria associada à transação
        public string Descricao { get; set; } = string.Empty; // Descrição da transação (ex: "Compra no supermercado", "Salário do mês")
        public decimal Valor { get; set; } // Valor da transação
        public DateTime DataTransacao { get; set; } = DateTime.UtcNow; // Data em que a transação ocorreu
    }
}