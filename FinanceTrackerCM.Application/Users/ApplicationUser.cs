using Microsoft.AspNetCore.Identity;

namespace FinanceTrackerCM.Application.Users;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid TenantId { get; set; }
}
