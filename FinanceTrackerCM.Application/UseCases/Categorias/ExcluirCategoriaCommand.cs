using MediatR;
namespace FinanceTrackerCM.Application.UseCases.Categorias;

public class ExcluirCategoriaCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
}
