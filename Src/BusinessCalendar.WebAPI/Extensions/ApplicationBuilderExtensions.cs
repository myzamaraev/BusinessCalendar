using BusinessCalendar.Postgres.Extensions;
using BusinessCalendar.WebAPI.Constants;
using BusinessCalendar.WebAPI.Options;
using FluentMigrator.Runner;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace BusinessCalendar.WebAPI.Extensions;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds health check endpoints
    /// </summary>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
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

    /// <summary>
    /// Triggers automatic migrations for databases with schema when storage layer provides IMigrationRunner
    /// </summary>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
    /// <returns></returns>
    public static IApplicationBuilder ApplyDatabaseMigrations(this IApplicationBuilder applicationBuilder)
    {
        using var scope = applicationBuilder.ApplicationServices.CreateScope();
        var storageOptions = scope.ServiceProvider.GetRequiredService<IOptions<StorageOptions>>().Value;
        if (storageOptions.AllowAutoMigrations)
        {
            var runner = scope.ServiceProvider.GetService<IMigrationRunner>(); 
            runner?.MigrateUp(); //Run migration if IMigrationRunner has provided via storage layer
        }

        return applicationBuilder;
    }
}