using MediatR;
using FinanceTrackerCM.Domain.Enums;

namespace FinanceTrackerCM.Application.UseCases.Categorias;

public class CriarCategoriaCommand : IRequest<Guid>
{ // Comando para criar uma nova categoria de transação financeira do usuário, que contém os dados necessários para a criação da categoria
    public string NomeCategoria { get; set; } = string.Empty;
    public TipoTransacao Tipo { get; set; }
}