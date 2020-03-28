using System;

namespace Framework.Auth
{
    public class UserContext
    {
        public bool IsAuthenticated { get; set; }

        public UserClaims Claims { get; set; }

        public UserContext()
        {
            Claims = new UserClaims();
        }
    }

    public class UserClaims
    {
        public Guid OnUserId { get; set; }

        public Guid RoleId { get; set; }
    }
}
