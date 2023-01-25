using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace DemoWebAppMvc
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserID(this ClaimsPrincipal User)
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public static string GetUserRole(this ClaimsPrincipal User)
        {
            return User.FindFirst(ClaimTypes.Role).Value;
        }
    }
}
