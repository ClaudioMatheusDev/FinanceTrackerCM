using FinanceTrackerCM.Domain.Enums;
using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerCM.Domain.Entities
{
    // Entidade que representa uma conta financeira do usuário
    // Exemplo de contas: "Conta Corrente", "Poupança", "Nubank", "Carteira"
    public class Conta
    {// Propriedades da entidade Conta, que representam os dados de uma conta financeira do usuário
        public Guid Id { get; set; } // Identificador único da conta
        public string NomeConta { get; set; } = string.Empty; // Nome da conta (ex: "Conta Corrente", "Poupança", "Nubank", "Carteira")
        [Column(TypeName = "decimal(18,2)")] // Configuração para o tipo decimal no banco de dados, garantindo precisão para valores monetários
        public decimal Saldo { get; set; } // Saldo atual da conta
        public StatusConta Status { get; set; } // Status da conta (Ativa ou Inativa)
        public Guid IdUsuario { get; set; } // Identificador do usuário proprietário da conta
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow; // Data de criação da conta
        public Guid TenantId { get; set; } // Identificador do tenant para suporte a multi-tenancy
    }

    public class ContaValidator : AbstractValidator<Conta>
    {
        public ContaValidator()
        {
            RuleFor(c => c.NomeConta)
                .NotEmpty().WithMessage("O nome da conta é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da conta deve ter no máximo 100 caracteres.");

            RuleFor(c => c.Saldo)
                .GreaterThanOrEqualTo(0).WithMessage("O saldo da conta não pode ser negativo.");

            RuleFor(c => c.Status)
                .IsInEnum().WithMessage("O status da conta deve ser Ativa ou Inativa.");
        }
    }
}