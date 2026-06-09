using FinanceTrackerCM.Domain.Enums;

namespace FinanceTrackerCM.Domain.Entities
{    // Entidade que representa uma categoria de transação financeira do usuário
    // Exemplo de categorias:

public class Categoria
    {   public Guid Id { get; set; } // Identificador único da categoria
        public string NomeCategoria { get; set; } = string.Empty; // Nome da categoria (ex: "Alimentação", "Transporte", "Lazer", "Salário")
        public TipoTransacao Tipo { get; set; } // Tipo da categoria (Receita ou Despesa)
        public Guid IdUsuario { get; set; } // Identificador do usuário proprietário da categoria
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow; // Data de criação da categoria
    }


}