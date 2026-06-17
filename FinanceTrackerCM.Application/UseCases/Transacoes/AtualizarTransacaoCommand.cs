using MediatR;

namespace FinanceTrackerCM.Application.UseCases.Transacoes
{
    public class AtualizarTransacaoCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public Guid ContaId { get; set; }
        public Guid CategoriaId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataTransacao { get; set; } = DateTime.UtcNow;
        public string Descricao { get; set; } = string.Empty;
    }
}