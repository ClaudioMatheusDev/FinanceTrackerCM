using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Domain.Enums;
using FinanceTrackerCM.Application.Interfaces;

namespace FinanceTrackerCM.Application.UseCases.Contas;

public class AtualizarContaHandler : IRequestHandler<AtualizarContaCommand, Guid>
{       
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;
    public AtualizarContaHandler(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {
        _context = context;
        _currentUserResolver = currentUserResolver;
    }
        public async Task<Guid> Handle(AtualizarContaCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserResolver.TenantId;

           // var conta = await _context.Contas
             //   .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken);
            var conta = await _context.Contas
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            if (conta == null)
                throw new InvalidOperationException("Conta não encontrada");

            conta.NomeConta = request.Nome;
            conta.Saldo = request.Saldo;
            conta.Status = request.Ativa ? StatusConta.Ativa : StatusConta.Inativa;

            await _context.SaveChangesAsync(cancellationToken);

            return conta.Id;
        }
}