using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Application.Interfaces;

namespace FinanceTrackerCM.Application.UseCases.Contas;
    public class ExcluirContaHandle : IRequestHandler<ExcluirContaCommand, Guid>
{       
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;
    public ExcluirContaHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {
        _context = context;
        _currentUserResolver = currentUserResolver;
    }
        public async Task<Guid> Handle(ExcluirContaCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserResolver.TenantId;
            var userId = _currentUserResolver.UserId;

            if (userId == Guid.Empty || tenantId == Guid.Empty)
                throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

            var conta = await _context.Contas
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken);


            if (conta == null)
                throw new InvalidOperationException("Conta não encontrada");

            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync(cancellationToken);

            return conta.Id;
        }
}
