using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string
                    Rol = "rol",
                    Id = "id",
                    fullName = "fullName",
                    avatar = "avatar",
                    email = "email",
                    username = "username",
                    rolesCore = "rolesCore",
                    permissions = "permissions";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }
        }
    }
}
