namespace FinanceTrackerCM.Application.Interfaces;

public interface ICurrentUserResolver
{
    Guid UserId { get; }
    Guid TenantId { get; }
    bool IsAuthenticated { get; }
}
