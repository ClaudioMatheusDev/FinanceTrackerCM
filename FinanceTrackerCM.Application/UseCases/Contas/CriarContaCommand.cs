using MediatR;
namespace FinanceTrackerCM.Application.UseCases.Contas;

public class CriarContaCommand : IRequest<Guid>
{ // Comando para criar uma nova conta financeira do usuário, que contém os dados necessários para a criação da conta
    public string Nome { get; set; } = string.Empty;
    public decimal SaldoInicial { get; set; }
}