using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace FinanceTrackerCM.Infrastructure.Services
{
    /// <summary>
    /// Implementação de ICurrentUserResolver para resolver o usuário atual a partir do contexto HTTP.
    /// </summary>

    public class CurrentUserResolver : FinanceTrackerCM.Application.Interfaces.ICurrentUserResolver, AuditLogCM.Core.Interfaces.ICurrentUserResolver
    {
        private readonly IHttpContextAccessor _accessor;
        public CurrentUserResolver(IHttpContextAccessor accessor) => _accessor = accessor;
        /// <summary>
        /// Obtém o ID do usuário atual a partir do claim "sub" presente no token JWT. Se o claim não estiver presente ou for inválido, retorna Guid.Empty.
        /// </summary>
        public Guid UserId
        {
            get
            {
                var id = _accessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
                return id is null ? Guid.Empty : Guid.Parse(id);
            }
        }
        /// <summary>
        /// Obtém o TenantId do usuário atual a partir do claim "TenantId" presente no token JWT.
        /// Se o claim não estiver presente ou for inválido, retorna Guid.Empty.
        /// </summary>
        public Guid TenantId
        {
            get
            {
                var t = _accessor.HttpContext?.User?.FindFirstValue("TenantId");
                return t is null ? Guid.Empty : Guid.Parse(t);
            }
        }

        public bool IsAuthenticated => _accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        /// <summary>
        /// Obtém o ID do usuário atual como string. Se o usuário não estiver autenticado ou o ID for inválido, retorna string.Empty.
        /// </summary>
        public string GetCurrentUserId()
        {
            return UserId == Guid.Empty ? string.Empty : UserId.ToString();
        }
        /// <summary>
        ///  Obtém o nome do usuário atual a partir do claim ClaimTypes.Name presente no token JWT. Se o claim não estiver presente, retorna string.Empty.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentUserName()
        {
            return _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        }
    }
}