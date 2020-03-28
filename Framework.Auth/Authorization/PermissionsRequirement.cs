using Microsoft.AspNetCore.Authorization;

namespace Framework.Auth
{
    public class PermissionsRequirement : IAuthorizationRequirement
    {
        public PermissionsRequirement(LogicalOperators logicalOperator, string[] allowedPermissions)
        {
            LogicalOperator = logicalOperator;
            AllowedPermissions = allowedPermissions;
        }

        public string[] AllowedPermissions { get; set; }

        public LogicalOperators LogicalOperator { get; set; }
    }

    public enum LogicalOperators
    {
        And,
        Or
    }
}
