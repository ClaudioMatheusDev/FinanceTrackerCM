using FinanceTrackerCM.Application.DTOs;
using MediatR;

namespace FinanceTrackerCM.Application.UseCases.Categorias
{
    public class ObterTodasCategoriasCommand : IRequest<IEnumerable<CategoriasDTO>>
    {
    }
}