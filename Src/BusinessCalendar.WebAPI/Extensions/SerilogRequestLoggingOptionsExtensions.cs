using Serilog.AspNetCore;

namespace BusinessCalendar.WebAPI.Extensions;

public static class SerilogRequestLoggingOptionsExtensions
{
    public static void EnrichRequestContext(this RequestLoggingOptions options)
    {
        options.IncludeQueryInRequestPath = true;
        options.EnrichDiagnosticContext = (context, httpContext) =>
        {
            var principal = httpContext.User;
            var isAuthenticated = principal?.Identity?.IsAuthenticated ?? false;
            context.Set("IsAuthenticated", isAuthenticated);
            if (isAuthenticated)
            {
                context.Set("User_Name", principal?.GetUserName() ?? string.Empty);
                context.Set("User_Email", principal?.GetEmail() ?? string.Empty);
                context.Set("User_Roles", principal?.GetRoles());
            }
        };
    }
}