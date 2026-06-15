using FinanceTrackerCM.Application.DTOs;
using MediatR;
namespace FinanceTrackerCM.Application.UseCases.Contas
{
    public class ObterTodasContasQuery : IRequest<IEnumerable<ContaDto>>
    {
    }
}
