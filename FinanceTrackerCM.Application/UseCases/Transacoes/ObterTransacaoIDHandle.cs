using MediatR;
using FinanceTrackerCM.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Application.DTOs;

namespace FinanceTrackerCM.Application.UseCases.Transacoes
{
    public class ObterTransacaoIDHandle : IRequestHandler<ObterTransacaoIDCommand, TransacaoDto>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserResolver _currentUserResolver;

        public ObterTransacaoIDHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
        {
            _context = context;
            _currentUserResolver = currentUserResolver;
        }

        public async Task<TransacaoDto> Handle(ObterTransacaoIDCommand request, CancellationToken cancellationToken)
        {
            var tenantId = _currentUserResolver.TenantId;
            var userId = _currentUserResolver.UserId;

            if (userId == Guid.Empty || tenantId == Guid.Empty)
                throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

            var transacao = await _context.Transacoes
                .FirstOrDefaultAsync(t => t.Id == request.Id && t.TenantId == tenantId, cancellationToken);

            if (transacao == null)
                throw new InvalidOperationException("Transação não encontrada");

        var transacaoDto = new TransacaoDto
        {
            Id = transacao.Id,
            ContaId = transacao.ContaId,
            CategoriaId = transacao.CategoriaId,
            IdUsuario = transacao.IdUsuario,
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
            Status = transacao.Status,
            Conta = transacao.Conta,
            Categoria = transacao.Categoria,
            Tipo = transacao.Tipo
        };

        return transacaoDto;
        }
    }
}
