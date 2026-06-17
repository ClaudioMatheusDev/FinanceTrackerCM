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
        public ObterTransacoesHandle(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransacaoDto>> Handle(ObterTransacoesCommand request, CancellationToken cancellationToken)
        {
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
                Tipo = t.Tipo
            }).ToListAsync(cancellationToken);
            return transacao;
        }
    }
}