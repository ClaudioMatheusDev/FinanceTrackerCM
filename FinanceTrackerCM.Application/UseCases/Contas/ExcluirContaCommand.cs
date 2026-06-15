using MediatR;
namespace FinanceTrackerCM.Application.UseCases.Contas;

public class ExcluirContaCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
}