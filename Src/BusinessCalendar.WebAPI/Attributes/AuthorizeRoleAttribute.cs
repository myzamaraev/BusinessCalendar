using Microsoft.AspNetCore.Authorization;

namespace BusinessCalendar.WebAPI.Attributes;

/// <summary>
/// Allows access to an endpoint only for users with at least one role from provided list
/// </summary>
public class AuthorizeRoleAttribute : AuthorizeAttribute
{
    public AuthorizeRoleAttribute(params string[] roles) : base()
    {
        Roles = string.Join(",", roles);
    }
}