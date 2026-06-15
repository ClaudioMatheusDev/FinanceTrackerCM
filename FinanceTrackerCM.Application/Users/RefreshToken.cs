namespace FinanceTrackerCM.Application.Users
{
    // Classe que representa um token de atualização (refresh token) para autenticação e autorização de usuários na aplicação
    public class RefreshToken
    {
// Propriedade para armazenar o identificador único do token de atualização, gerado automaticamente como um novo GUID
        public Guid Id { get; set; } = Guid.NewGuid(); // Propriedade para armazenar o identificador do usuário ao qual o token de atualização pertence, permitindo a associação entre o token e o usuário correspondente
        public Guid UserId { get; set; } // Propriedade para armazenar o valor do token de atualização, que é uma string gerada aleatoriamente e usada para obter um novo token de acesso quando o token de acesso atual expira
        public string Token { get; set; } = string.Empty; // Propriedade para armazenar a data e hora de expiração do token de atualização, indicando até quando o token é válido para uso
        public DateTime ExpiresAt { get; set; } // Propriedade para indicar se o token de atualização foi revogado, ou seja, se ele não é mais válido para uso, permitindo a implementação de mecanismos de revogação de tokens na aplicação
        public bool Revoked { get; set; } // Propriedade para armazenar a data e hora em que o token de atualização foi criado, permitindo rastrear quando o token foi gerado e implementar políticas de expiração ou rotação de tokens com base nessa informação
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Propriedade para armazenar a data e hora em que o token de atualização foi revogado, permitindo rastrear quando o token foi invalidado e implementar políticas de revogação de tokens com base nessa informação

    }
}