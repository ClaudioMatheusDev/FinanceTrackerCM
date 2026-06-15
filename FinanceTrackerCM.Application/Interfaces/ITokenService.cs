using FinanceTrackerCM.Application.Users;

namespace FinanceTrackerCM.Application.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(ApplicationUser user);
    RefreshToken CreateRefreshToken(Guid userId, int days);
}