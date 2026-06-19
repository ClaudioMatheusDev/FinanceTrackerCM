using MediatR;
using FinanceTrackerCM.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Application.Interfaces;

namespace FinanceTrackerCM.Application.UseCases.Transacoes
{
    // Classe que representa o handler
    public class ObterTransacoesHandle : IRequestHandler<ObterTransacoesCommand, IEnumerable<TransacaoDto>>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserResolver _currentUserResolver;
        public ObterTransacoesHandle(IAppDbContext context, ICurrentUserResolver currentUserResolver)
        {
            _context = context;
            _currentUserResolver = currentUserResolver;
        }

        public async Task<IEnumerable<TransacaoDto>> Handle(ObterTransacoesCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserResolver.UserId == Guid.Empty || _currentUserResolver.TenantId == Guid.Empty)
                throw new UnauthorizedAccessException("Usuário ou tenant não identificado.");

            var transacao = await _context.Transacoes.Select(t => new TransacaoDto
            {
                Id = t.Id,
                ContaId = t.ContaId,
                CategoriaId = t.CategoriaId,
                IdUsuario = t.IdUsuario,
                Descricao = t.Descricao,
                Valor = t.Valor,
                Status = t.Status,
                Conta = t.Conta,
                Categoria = t.Categoria,
                Tipo = t.Tipo,
                DataTransacao = t.DataTransacao,
                TenantId = t.TenantId
            }).ToListAsync(cancellationToken);
            return transacao;
        }
    }
}
