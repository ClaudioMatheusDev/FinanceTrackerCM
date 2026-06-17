using MediatR;
using FinanceTrackerCM.Application.DTOs;

namespace FinanceTrackerCM.Application.UseCases.Transacoes
{
    // Classe que representa um comando para obter
public class ObterTransacaoIDCommand : IRequest<TransacaoDto>
    {
        public Guid Id { get; set; }
    }

}    