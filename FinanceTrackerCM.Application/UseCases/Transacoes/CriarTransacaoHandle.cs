    using MediatR;
    using FinanceTrackerCM.Application.Interfaces;
    using Microsoft.EntityFrameworkCore;    

    namespace FinanceTrackerCM.Application.UseCases.Transacoes
    {

        public class CriarTransacaoHandle : IRequestHandler<CriarTransacaoCommand, Guid>
        {
            private readonly IAppDbContext _context;

            private readonly ICurrentUserResolver _currentUserResolver;

            public CriarTransacaoHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
            {
                _context = context;
                _currentUserResolver = currentUserResolver;
            }

            public async Task<Guid> Handle(CriarTransacaoCommand request, CancellationToken cancellationToken)
            {
                var tenantId = _currentUserResolver.TenantId;

                        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c =>
                c.Id == request.CategoriaId &&
                c.TenantId == tenantId,
                cancellationToken);

                var contaExiste = await _context.Contas
                    .AnyAsync(c =>
                        c.Id == request.ContaId &&
                        c.TenantId == tenantId,
                        cancellationToken);

                var transacao = new Domain.Entities.Transacao
                {
                    Id = Guid.NewGuid(),
                    ContaId = request.ContaId,
                    CategoriaId = request.CategoriaId,
                    Valor = request.Valor,
                    DataTransacao = request.DataTransacao,
                    Descricao = request.Descricao,
                    TenantId = tenantId
                };

                _context.Transacoes.Add(transacao);
                await _context.SaveChangesAsync(cancellationToken);

                return transacao.Id;
            }

        }
    }