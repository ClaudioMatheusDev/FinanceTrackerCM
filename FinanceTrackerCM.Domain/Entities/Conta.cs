using FinanceTrackerCM.Domain.Enums;

namespace FinanceTrackerCM.Domain.Entities
{
    // Entidade que representa uma conta financeira do usuário
    // Exemplo de contas: "Conta Corrente", "Poupança", "Nubank", "Carteira"
    public class Conta
    {// Propriedades da entidade Conta, que representam os dados de uma conta financeira do usuário
        public Guid Id { get; set; } // Identificador único da conta
        public string NomeConta { get; set; } = string.Empty; // Nome da conta (ex: "Conta Corrente", "Poupança", "Nubank", "Carteira")
        public decimal Saldo { get; set; } // Saldo atual da conta
        public StatusConta Status { get; set; } // Status da conta (Ativa ou Inativa)
        public Guid IdUsuario { get; set; } // Identificador do usuário proprietário da conta
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow; // Data de criação da conta
    }
}