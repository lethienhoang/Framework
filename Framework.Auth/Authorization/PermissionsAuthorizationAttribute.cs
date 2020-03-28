using Microsoft.AspNetCore.Authorization;

namespace Framework.Auth
{
    public class PermissionsAuthorizationAttribute : AuthorizeAttribute
    {
        private readonly string[] _allowedPermissions;
        private readonly LogicalOperators _logicalOperator;

        public PermissionsAuthorizationAttribute(LogicalOperators logicalOperator, params string[] allowedPermissions)
        {
            _allowedPermissions = allowedPermissions;
            _logicalOperator = logicalOperator;

            Permissions = new PermissionsRequirement(_logicalOperator, _allowedPermissions);
        }

        public PermissionsRequirement Permissions
        {
            get
            {
                return new PermissionsRequirement(_logicalOperator, _allowedPermissions);
            }
            set
            {
                var policy = $"{_logicalOperator}___{string.Join("___", _allowedPermissions)}";
                Policy = $"{PrefixConstants.PERMISSIONS_POLICY_PREFIX}{policy}";
            }
        }
    }
}
