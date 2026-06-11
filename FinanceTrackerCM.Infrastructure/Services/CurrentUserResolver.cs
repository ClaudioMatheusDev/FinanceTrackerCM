using Microsoft.AspNetCore.Http;

namespace FinanceTrackerCM.Infrastructure.Services
{
    public class CurrentUserResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
        }
    }
}