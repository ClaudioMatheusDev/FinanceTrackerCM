using MediatR;
namespace FinanceTrackerCM.Application.UseCases.Contas;

public class AtualizarContaCommand : IRequest<Guid>
{ // Comando para atualizar os dados de uma conta financeira do usuário, que contém os dados necessários para a atualização da conta
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
    public bool Ativa { get; set; }
}