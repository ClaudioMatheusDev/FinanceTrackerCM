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

        public ObterTodasContasQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContaDto>> Handle(
            ObterTodasContasQuery request,
            CancellationToken cancellationToken)
        {
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
