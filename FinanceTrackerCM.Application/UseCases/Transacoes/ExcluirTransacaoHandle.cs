using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Application.Interfaces;

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
            throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

        var transacao = await _context.Transacoes
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken);

        if (transacao == null)
            throw new InvalidOperationException("Transação não encontrada");

        _context.Transacoes.Remove(transacao);
        await _context.SaveChangesAsync(cancellationToken);
        return transacao.Id;   
    }
}                
