using FinanceTrackerCM.Domain.Enums;
using FluentValidation;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceTrackerCM.Domain.Entities
{    // Entidade que representa uma categoria de transação financeira do usuário
     // Exemplo de categorias:

    public class Transacao
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; } // Valor da transação
        public DateTime DataTransacao { get; set; } = DateTime.UtcNow; // Data em que a transação ocorreu
        public Guid TenantId { get; set; } // Identificador do tenant para suporte a multi-tenancy
    }

    public class TransacaoValidator : AbstractValidator<Transacao>
    {
        public TransacaoValidator()
        {
            RuleFor(t => t.Descricao)
                .NotEmpty().WithMessage("A descrição da transação é obrigatória.")
                .MaximumLength(200).WithMessage("A descrição da transação deve ter no máximo 200 caracteres.");

            RuleFor(t => t.Valor)
                .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");
        }
    }
}