using MediatR;
using FinanceTrackerCM.Application.DTOs;
namespace FinanceTrackerCM.Application.UseCases.Contas;

public class ObterContaPorIdQuery : IRequest<ContaDto>
{ // Consulta para obter os detalhes de uma conta financeira específica do usuário, que contém o Id da conta a ser obtida
    public Guid Id { get; set; }
}