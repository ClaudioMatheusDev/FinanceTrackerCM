using MediatR;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Application.Interfaces;


namespace FinanceTrackerCM.Application.UseCases.Categorias;

public class ExcluirCategoriaHandler : IRequestHandler<ExcluirCategoriaCommand, Guid>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;

    public ExcluirCategoriaHandler(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {
        _context = context;
        _currentUserResolver = currentUserResolver;
    }

    public async Task<Guid> Handle(ExcluirCategoriaCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserResolver.TenantId;

        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken);

        if (categoria == null)
            throw new InvalidOperationException("Categoria não encontrada");

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync(cancellationToken);

        return categoria.Id;
    }
}