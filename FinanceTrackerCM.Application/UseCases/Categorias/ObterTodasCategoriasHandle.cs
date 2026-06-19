using FinanceTrackerCM.Application.DTOs;
using FinanceTrackerCM.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerCM.Application.UseCases.Categorias
{
    public class ObterTodasCategoriasHandle : IRequestHandler<ObterTodasCategoriasCommand, IEnumerable<CategoriasDTO>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserResolver _currentUserResolver;

        public ObterTodasCategoriasHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
        {
            _context = context;
            _currentUserResolver = currentUserResolver;
        }

        public async Task<IEnumerable<CategoriasDTO>> Handle(
            ObterTodasCategoriasCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserResolver.UserId == Guid.Empty || _currentUserResolver.TenantId == Guid.Empty)
                throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

            var categorias = await _context.Categorias.Select(c => new CategoriasDTO{
                Id = c.Id,
                NomeCategoria = c.NomeCategoria,
                Tipo = c.Tipo,
                IdUsuario = c.IdUsuario,
                DataCriacao = c.DataCriacao,
                TenantId = c.TenantId
            }).ToListAsync(cancellationToken);
        
        return categorias;
        }

    }
}
