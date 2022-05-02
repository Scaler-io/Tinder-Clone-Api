using System;
using System.Security.Claims;

namespace Tinder_Dating_API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetAuthUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static Guid GetAuthUserId(this ClaimsPrincipal user)
        {
            var id =  user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(id);
        }
    }
}
