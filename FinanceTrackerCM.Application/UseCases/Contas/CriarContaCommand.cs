using MediatR;
namespace FinanceTrackerCM.Application.UseCases.Contas;

public class CriarContaCommand : IRequest<Guid>
{
    public string Nome { get; set; } = string.Empty;
    public decimal SaldoInicial { get; set; }

}