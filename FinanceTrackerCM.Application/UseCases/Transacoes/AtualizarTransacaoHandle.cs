using MediatR;
using FinanceTrackerCM.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Domain.Entities;
using FluentValidation;

namespace FinanceTrackerCM.Application.UseCases.Transacoes
{
    public class AtualizarTransacaoHandle :  IRequestHandler<AtualizarTransacaoCommand, Guid>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserResolver _currentUserResolver;
        public AtualizarTransacaoHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
        {
            _context = context;
            _currentUserResolver = currentUserResolver;
        }

        public async Task<Guid> Handle(AtualizarTransacaoCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserResolver.TenantId;
            var userId = _currentUserResolver.UserId;

            if (userId == Guid.Empty || tenantId == Guid.Empty)
                throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

            var transacao = await _context.Transacoes
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.TenantId == tenantId, cancellationToken);

            if (transacao == null)
                throw new InvalidOperationException("Transação não encontrada");

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == request.CategoriaId && c.TenantId == tenantId, cancellationToken);

            if (categoria == null)
                throw new InvalidOperationException("Categoria não encontrada para o tenant atual.");

            var contaExiste = await _context.Contas
                .AnyAsync(c => c.Id == request.ContaId && c.TenantId == tenantId, cancellationToken);

            if (!contaExiste)
                throw new InvalidOperationException("Conta não encontrada para o tenant atual.");

            transacao.ContaId = request.ContaId;
            transacao.CategoriaId = request.CategoriaId;
            transacao.Valor = request.Valor;
            transacao.DataTransacao = request.DataTransacao;
            transacao.Descricao = request.Descricao;
            transacao.Tipo = categoria.Tipo;

            new TransacaoValidator().ValidateAndThrow(transacao);

            await _context.SaveChangesAsync(cancellationToken);
            return transacao.Id;
        }
    }
}
