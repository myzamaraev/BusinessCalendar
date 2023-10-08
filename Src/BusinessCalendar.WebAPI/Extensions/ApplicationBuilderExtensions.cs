using BusinessCalendar.WebAPI.Constants;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace BusinessCalendar.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds health check endpoints
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <returns></returns>
    public static IApplicationBuilder MapHealthcheckEndpoints(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks(
                "/health/ready",
                new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(HealthConstants.ReadyTag),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            endpoints.MapHealthChecks(
                "/health/live",
                new HealthCheckOptions()
                {
                    Predicate = check => check.Tags.Contains(HealthConstants.LiveTag),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
        });
        return applicationBuilder;
    }
}