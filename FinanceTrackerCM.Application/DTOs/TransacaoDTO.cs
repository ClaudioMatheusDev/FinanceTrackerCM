using FinanceTrackerCM.Domain.Enums;
using FinanceTrackerCM.Domain.Entities;

namespace FinanceTrackerCM.Application.DTOs
{   // DTO (Data Transfer Object) para representar os dados de uma conta financeira do usuário, que será utilizado para transferir os dados entre as camadas da aplicação (ex: do handler para o controlador)
    public class TransacaoDto
    {   // Propriedades da entidade Transacao, que representam os dados de uma transação financeira do usuário
        public Guid Id { get; set; } // Identificador único da transação
        public Guid ContaId { get; set; } // Identificador da conta associada à transação
        public Guid CategoriaId { get; set; } // Identificador da categoria associada à transação
        public Guid IdUsuario { get; set; } // Identificador do usuário proprietário da transação
        public TipoTransacao Tipo { get; set; } // Tipo da transação (Receita ou Despesa)
        public StatusTransacao Status { get; set; }// Status da transação (Pendente, Concluída, Cancelada)
        public Conta Conta { get; set; } = null!; // Conta associada à transação
        public Categoria Categoria { get; set; } = null!; // Categoria associada à transação
        public string Descricao { get; set; } = string.Empty; // Descrição da transação (ex: "Compra no supermercado", "Salário do mês")
        public decimal Valor { get; set; } // Valor da transação
        public DateTime DataTransacao { get; set; } = DateTime.UtcNow; // Data em que a transação ocorreu
        public Guid TenantId { get; set; } // Identificador do tenant para suporte a multi-tenancy
    }
}