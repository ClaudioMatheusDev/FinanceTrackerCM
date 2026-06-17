using MediatR;
using FinanceTrackerCM.Domain.Enums;


namespace FinanceTrackerCM.Application.UseCases.Categorias;

public class AtualizarCategoriaCommand : IRequest<Guid>
{ // Comando para atualizar os dados de uma categoria financeira do usuário, que contém os dados necessários para a atualização da categoria
    public Guid Id { get; set; }
    public string NomeCategoria { get; set; } = string.Empty;
    public TipoTransacao Tipo { get; set; }
}