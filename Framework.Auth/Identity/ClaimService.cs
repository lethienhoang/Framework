using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Framework.Auth
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ClaimService> _logger;

        public ClaimService(
            IHttpContextAccessor httpContextAccessor,
            ILogger<ClaimService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public UserContext GetUserContext()
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return new UserContext();
            }

            var OnUserId = user.Claims.SingleOrDefault(x => x.Type == CoreConstants.OnUserIdClaimType)?.Value;
            var roleId = user.Claims.SingleOrDefault(x => x.Type == CoreConstants.RoleIdClaimType)?.Value;

            if (string.IsNullOrEmpty(OnUserId) && string.IsNullOrEmpty(roleId))
            {
                return new UserContext();
            }

            var userClaims = new UserClaims
            {
                OnUserId = Guid.Parse(OnUserId),
                RoleId = Guid.Parse(roleId)
            };

            return new UserContext()
            {
                IsAuthenticated = user.Identity.IsAuthenticated,
                Claims = userClaims,
            };
        }
    }
}
