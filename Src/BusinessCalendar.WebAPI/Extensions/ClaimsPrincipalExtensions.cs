using System.Security.Claims;
using BusinessCalendar.WebAPI.Constants;

namespace BusinessCalendar.WebAPI.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserName(this ClaimsPrincipal principal)
    {
        return principal?.Identity?.Name;
    }
    
    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal?.FindFirst("email")?.Value;
    }
    
    public static List<string> GetRoles(this ClaimsPrincipal principal)
    {
        if (principal == null)
        {
            return new List<string>();
        }
        
        return principal
            .FindAll("role")
            .Select(x => x.Value)
            .Distinct()
            .Intersect(BcRoles.RoleList)
            .ToList();
    }
}