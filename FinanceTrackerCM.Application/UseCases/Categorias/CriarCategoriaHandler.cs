using MediatR;
using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Domain.Entities;

namespace FinanceTrackerCM.Application.UseCases.Categorias;

public class CriarCategoriaHandler : IRequestHandler<CriarCategoriaCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;

    public CriarCategoriaHandler(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {
        _context = context;
        _currentUserResolver = currentUserResolver;
    }

        public async Task<Guid> Handle(CriarCategoriaCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserResolver.TenantId;
            var userId = _currentUserResolver.UserId;

            if (userId == Guid.Empty)
                throw new UnauthorizedAccessException("Usuário não autenticado.");

        var categoria = new Categoria
        {
            Id = Guid.NewGuid(),
            NomeCategoria = request.NomeCategoria,
            IdUsuario = userId,
            TenantId = tenantId
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync(cancellationToken);

        return categoria.Id;
    }
}