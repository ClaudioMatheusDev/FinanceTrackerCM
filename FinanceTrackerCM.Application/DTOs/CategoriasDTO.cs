using FinanceTrackerCM.Domain.Enums;

namespace FinanceTrackerCM.Application.DTOs
{   // DTO (Data Transfer Object) para representar os dados de uma conta financeira do usuário, que será utilizado para transferir os dados entre as camadas da aplicação (ex: do handler para o controlador)
    public class CategoriasDTO
    {   // Propriedades da entidade Transacao, que representam os dados de uma transação financeira do usuário
        public Guid Id { get; set; } // Identificador único da categoria
        public string NomeCategoria { get; set; } = string.Empty; // Nome da categoria (ex: "Alimentação", "Transporte", "Lazer", "Salário")
        public TipoTransacao Tipo { get; set; } // Tipo da categoria (Receita ou Despesa)
        public Guid IdUsuario { get; set; } // Identificador do usuário proprietário da categoria
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow; // Data de criação da categoria
        public Guid TenantId { get; set; } // Identificador do tenant para suporte a multi-tenancy
    }
}