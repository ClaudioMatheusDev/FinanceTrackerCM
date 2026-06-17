using MediatR;
using FinanceTrackerCM.Application.Interfaces;
using FinanceTrackerCM.Domain.Entities;
using FinanceTrackerCM.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;


namespace FinanceTrackerCM.Application.UseCases.Categorias;

public class AtualizarCategoriaHandler : IRequestHandler<AtualizarCategoriaCommand, Guid>
{ // Handler para processar o comando de atualização de uma categoria financeira do usuário, que contém a lógica para atualizar a categoria no banco de dados
    private readonly IAppDbContext _context;
    private readonly ICurrentUserResolver _currentUserResolver;

    public AtualizarCategoriaHandler(IAppDbContext context, ICurrentUserResolver currentUserResolver)
    {
        _context = context;
        _currentUserResolver = currentUserResolver;
    }

    public async Task<Guid> Handle(AtualizarCategoriaCommand request, CancellationToken cancellationToken)
    {
        var tenantId = _currentUserResolver.TenantId;

        // categoria = await _context.Categorias
        //  .FirstOrDefaultAsync(x => x.Id == request.Id && x.TenantId == tenantId, cancellationToken);
        var categoria = await _context.Categorias
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (categoria == null)
            throw new InvalidOperationException("Categoria não encontrada");

        categoria.NomeCategoria = request.NomeCategoria;
        categoria.Tipo = request.Tipo;

        await _context.SaveChangesAsync(cancellationToken);

        return categoria.Id;
    }
}