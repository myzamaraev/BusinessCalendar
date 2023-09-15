using Microsoft.AspNetCore.Authorization;

namespace BusinessCalendar.WebAPI.Attributes;

public class AuthorizeRoleAttribute : AuthorizeAttribute
{
    public AuthorizeRoleAttribute(params string[] roles) : base()
    {
        Roles = string.Join(",", roles);
    }
}