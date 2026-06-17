using MediatR;
using FinanceTrackerCM.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using FinanceTrackerCM.Application.DTOs;

namespace FinanceTrackerCM.Application.UseCases.Transacoes
{
    public class ObterTransacaoIDHandle : IRequestHandler<ObterTransacaoIDCommand, TransacaoDto>
    {
        private readonly IAppDbContext _context;

        public ObterTransacaoIDHandle(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<TransacaoDto> Handle(ObterTransacaoIDCommand request, CancellationToken cancellationToken)
        {
            var transacao = await _context.Transacoes
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (transacao == null)
            {
                throw new Exception($"Transação não encontrada. Id={request.Id}");
            }

        var TransacaoDto = new TransacaoDto
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

        return TransacaoDto;
        }
    }
}