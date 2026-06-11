using MediatR;
using FinanceTrackerCM.Domain.Entities;
using FinanceTrackerCM.Domain.Enums;
using FinanceTrackerCM.Application.Interfaces;

namespace FinanceTrackerCM.Application.UseCases.Contas;

public class CriarContaHandler : IRequestHandler<CriarContaCommand, Guid>
{
    private readonly IAppDbContext _context;

    public CriarContaHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CriarContaCommand request, CancellationToken cancellationToken)
    {
        var novaConta = new Conta
        {
            Id = Guid.NewGuid(),
            NomeConta = request.Nome,
            Saldo = request.SaldoInicial,
            Status = StatusConta.Ativa,
            IdUsuario = Guid.NewGuid() 
        };

        _context.Contas.Add(novaConta);
        await _context.SaveChangesAsync(cancellationToken);

        return novaConta.Id;
    }
}

