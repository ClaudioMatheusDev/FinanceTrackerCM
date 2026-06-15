using Microsoft.AspNetCore.Identity;

namespace FinanceTrackerCM.Application.Users
{

    // Namespace que agrupa as classes relacionadas aos usuários da aplicação, incluindo a classe ApplicationUser que representa o usuário do sistema
    // Classe que representa o usuário da aplicação, estendendo a classe IdentityUser do ASP.NET Core Identity 
    // para incluir propriedades adicionais, como o TenantId para suporte a multi-tenancy
    public class ApplicationUser : IdentityUser<Guid>
    { // Propriedade para armazenar o identificador do locatário (tenant) ao qual o usuário pertence, permitindo a implementação de multi-tenancy na aplicação
        public Guid TenantId { get; set; }
    }
}