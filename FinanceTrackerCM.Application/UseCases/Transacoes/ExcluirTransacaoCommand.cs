using MediatR;
namespace FinanceTrackerCM.Application.UseCases.Transacoes;

public class ExcluirTransacaoCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
}
