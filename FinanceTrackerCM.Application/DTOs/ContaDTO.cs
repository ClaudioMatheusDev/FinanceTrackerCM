namespace FinanceTrackerCM.Application.DTOs
{   // DTO (Data Transfer Object) para representar os dados de uma conta financeira do usuário, que será utilizado para transferir os dados entre as camadas da aplicação (ex: do handler para o controlador)
    public class ContaDto
    {
        public Guid Id { get; set; } // Identificador único da conta financeira
        public string Nome { get; set; } = string.Empty; // Nome da conta financeira
        public decimal Saldo { get; set; } // Saldo atual da conta financeira
        public string Status { get; set; } = string.Empty; // Status da conta financeira (ex: Ativa, Inativa)
        public Guid IdUsuario { get; set; } // Identificador do usuário proprietário da conta financeira
    }
}