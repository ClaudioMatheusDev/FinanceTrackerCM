using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Application.Interfaces;

namespace FinanceTrackerCM.Application.UseCases.Contas;
    public class ExcluirContaHandler : IRequestHandler<ExcluirContaCommand, Guid>
{       
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;
    public ExcluirContaHandler(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {
        _context = context;
        _currentUserResolver = currentUserResolver;
    }
        public async Task<Guid> Handle(ExcluirContaCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserResolver.TenantId;

           // var conta = await _context.Contas
             //   .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken);
            var conta = await _context.Contas
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            if (conta == null)
                throw new InvalidOperationException("Conta não encontrada");

            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync(cancellationToken);

            return conta.Id;
        }
}