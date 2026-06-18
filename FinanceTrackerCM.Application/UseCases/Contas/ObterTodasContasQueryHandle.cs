using FinanceTrackerCM.Application.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Application.Interfaces;

namespace FinanceTrackerCM.Application.UseCases.Contas
{
    public class ObterTodasContasQueryHandler
        : IRequestHandler<ObterTodasContasQuery, IEnumerable<ContaDto>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserResolver _currentUserResolver;

        public ObterTodasContasQueryHandler(IAppDbContext context, ICurrentUserResolver currentUserResolver)
        {
            _context = context;
            _currentUserResolver = currentUserResolver;
        }

        public async Task<IEnumerable<ContaDto>> Handle(
            ObterTodasContasQuery request,
            CancellationToken cancellationToken)
        {
            if (_currentUserResolver.UserId == Guid.Empty || _currentUserResolver.TenantId == Guid.Empty)
                throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

            var contas = await _context.Contas
                .Select(c => new ContaDto
                {
                    Id = c.Id,
                    Nome = c.NomeConta,
                    Saldo = c.Saldo,
                    Status = c.Status.ToString(),
                    IdUsuario = c.IdUsuario
                })
                .ToListAsync(cancellationToken);

            return contas;
        }
    }
}
