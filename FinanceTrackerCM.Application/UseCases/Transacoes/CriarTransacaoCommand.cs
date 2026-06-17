using MediatR;

namespace FinanceTrackerCM.Application.UseCases.Transacoes
{
    // Classe que representa um token de atualização (refresh token) para autenticação e autorização de usuários na aplicação
    public class CriarTransacaoCommand : IRequest<Guid>
    {
        public Guid ContaId { get; set; }
        
        public Guid CategoriaId { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataTransacao { get; set; } = DateTime.UtcNow;
        public string Descricao { get; set; } = string.Empty;
    }
}