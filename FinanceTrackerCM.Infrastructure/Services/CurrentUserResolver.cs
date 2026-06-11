using Microsoft.AspNetCore.Http;
using AuditLogCM.Core.Interfaces;

namespace FinanceTrackerCM.Infrastructure.Services
{
    public class CurrentUserResolver : ICurrentUserResolver
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
        public string? GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value;
        }
    }
}