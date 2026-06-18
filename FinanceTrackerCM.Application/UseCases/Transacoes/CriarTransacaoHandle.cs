    using MediatR;
    using FinanceTrackerCM.Application.Interfaces;
    using Microsoft.EntityFrameworkCore;    
    using FinanceTrackerCM.Domain.Enums;
    using FinanceTrackerCM.Domain.Entities;
    using FluentValidation;

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
                var userId = _currentUserResolver.UserId;

                if (userId == Guid.Empty || tenantId == Guid.Empty)
                    throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

                var categoria = await _context.Categorias
                    .FirstOrDefaultAsync(c =>
                        c.Id == request.CategoriaId &&
                        c.TenantId == tenantId,
                        cancellationToken);

                if (categoria == null)
                    throw new InvalidOperationException("Categoria não encontrada para o tenant atual.");

                var contaExiste = await _context.Contas
                    .AnyAsync(c =>
                        c.Id == request.ContaId &&
                        c.TenantId == tenantId,
                        cancellationToken);

                if (!contaExiste)
                    throw new InvalidOperationException("Conta não encontrada para o tenant atual.");

                var transacao = new Transacao
                {
                    Id = Guid.NewGuid(),
                    ContaId = request.ContaId,
                    CategoriaId = request.CategoriaId,
                    Valor = request.Valor,
                    DataTransacao = request.DataTransacao,
                    Descricao = request.Descricao,
                    IdUsuario = userId,
                    Tipo = categoria.Tipo,
                    Status = StatusTransacao.Pendente,
                    TenantId = tenantId
                };

                new TransacaoValidator().ValidateAndThrow(transacao);

                _context.Transacoes.Add(transacao);
                await _context.SaveChangesAsync(cancellationToken);

                return transacao.Id;
            }

        }
    }
