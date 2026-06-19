using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerCM.Application.UseCases.Transacoes;

public class ExcluirTransacaoHandle : IRequestHandler<ExcluirTransacaoCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;

    public ExcluirTransacaoHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {
        _context = context;
        _currentUserResolver = currentUserResolver;
    }

    public async Task<Guid> Handle(ExcluirTransacaoCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserResolver.TenantId;
        var userId = _currentUserResolver.UserId;

        if (userId == Guid.Empty || tenantId == Guid.Empty)
            throw new UnauthorizedAccessException("Usuario ou tenant nao identificado.");

        var transacao = await _context.Transacoes
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken);

        if (transacao == null)
            throw new InvalidOperationException("Transacao nao encontrada");

        var conta = await _context.Contas
            .FirstOrDefaultAsync(c => c.Id == transacao.ContaId && c.TenantId == tenantId, cancellationToken);

        if (conta == null)
            throw new InvalidOperationException("Conta da transacao nao encontrada.");

        conta.Saldo -= GetSaldoDelta(transacao.Tipo, transacao.Valor);

        _context.Transacoes.Remove(transacao);
        await _context.SaveChangesAsync(cancellationToken);

        return transacao.Id;
    }

    private static decimal GetSaldoDelta(TipoTransacao tipo, decimal valor)
    {
        return tipo == TipoTransacao.Receita ? valor : -valor;
    }
}
