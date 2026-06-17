using FinanceTrackerCM.Domain.Enums;
using FluentValidation;

namespace FinanceTrackerCM.Domain.Entities
{    // Entidade que representa uma categoria de transação financeira do usuário
     // Exemplo de categorias:

    public class Categoria
    {   // Propriedades da entidade Categoria, que representam os dados de uma categoria de transação financeira do usuário
        public Guid Id { get; set; } // Identificador único da categoria
        public string NomeCategoria { get; set; } = string.Empty; // Nome da categoria (ex: "Alimentação", "Transporte", "Lazer", "Salário")
        public TipoTransacao Tipo { get; set; } // Tipo da categoria (Receita ou Despesa)
        public Guid IdUsuario { get; set; } // Identificador do usuário proprietário da categoria
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow; // Data de criação da categoria
        public Guid TenantId { get; set; } // Identificador do tenant para suporte a multi-tenancy
    }

    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        public CategoriaValidator()
        {
            RuleFor(c => c.NomeCategoria)
                .NotEmpty().WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da categoria deve ter no máximo 100 caracteres.");

            RuleFor(c => c.Tipo)
                .IsInEnum().WithMessage("O tipo da categoria deve ser Receita ou Despesa.");
        }
    }


}