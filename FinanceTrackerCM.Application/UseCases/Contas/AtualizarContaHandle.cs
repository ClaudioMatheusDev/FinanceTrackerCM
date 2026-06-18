using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Domain.Enums;
using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Domain.Entities;
using FluentValidation;

namespace FinanceTrackerCM.Application.UseCases.Contas;

public class AtualizarContaHandle : IRequestHandler<AtualizarContaCommand, Guid>
{       
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;
    public AtualizarContaHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {
        _context = context;
        _currentUserResolver = currentUserResolver;
    }
        public async Task<Guid> Handle(AtualizarContaCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserResolver.TenantId;
            var userId = _currentUserResolver.UserId;

            if (userId == Guid.Empty || tenantId == Guid.Empty)
                throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

            var conta = await _context.Contas
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken);


            if (conta == null)
                throw new InvalidOperationException("Conta não encontrada");

            conta.NomeConta = request.Nome;
            conta.Saldo = request.Saldo;
            conta.Status = request.Ativa ? StatusConta.Ativa : StatusConta.Inativa;

            new ContaValidator().ValidateAndThrow(conta);

            await _context.SaveChangesAsync(cancellationToken);

            return conta.Id;
        }
}
