using MediatR;
using FinanceTrackerCM.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceTrackerCM.Application.UseCases.Transacoes
{
    public class AtualizarTransacaoHandle :  IRequestHandler<AtualizarTransacaoCommand, Guid>
    {
        private readonly IAppDbContext _context;
        public AtualizarTransacaoHandle(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(AtualizarTransacaoCommand request, CancellationToken cancellationToken)
        {
            var transacao = await _context.Transacoes
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (transacao == null)
            {
                throw new Exception($"Transação não encontrada. Id={request.Id}");
            }

            transacao.ContaId = request.ContaId;
            transacao.CategoriaId = request.CategoriaId;
            transacao.Valor = request.Valor;
            transacao.DataTransacao = request.DataTransacao.AddHours(-3);
            transacao.Descricao = request.Descricao;

            await _context.SaveChangesAsync(cancellationToken);
            return transacao.Id;
        }
    }
}