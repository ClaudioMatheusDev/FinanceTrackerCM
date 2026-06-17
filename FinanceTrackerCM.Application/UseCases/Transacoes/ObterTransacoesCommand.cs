using MediatR;
using FinanceTrackerCM.Application.DTOs;

namespace FinanceTrackerCM.Application.UseCases.Transacoes
{
    // Classe que representa um comando para obter todas as transações
    public class ObterTransacoesCommand : IRequest<IEnumerable<TransacaoDto>>
    {
    }
}