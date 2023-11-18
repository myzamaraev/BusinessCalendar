using BusinessCalendar.WebAPI.Constants;
using BusinessCalendar.WebAPI.Options;
using FluentMigrator.Runner;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Prometheus;

namespace BusinessCalendar.WebAPI.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Maps all API endpoints including SPA, health checks, swagger documentation and metrics
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var actionEndpointBuilder = app.MapControllers();
        var authSettings = app.Services.GetRequiredService<IOptions<AuthOptions>>().Value;
        if (authSettings is { UseOpenIdConnectAuth: false })
        {
            actionEndpointBuilder.AllowAnonymous(); //Bypassing Auth with AllowAnonymousAttribute according to https://stackoverflow.com/a/62193352
        }

        //Swagger for public API endpoints is available in production by design.
        //in this case BFF endpoints are filtered by PublicApiDocumentFilter
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapHealthcheckEndpoints();
        app.MapMetrics(); //prometheus /metrics endpoint
        app.UseHttpMetrics(); //default asp.net core metrics
        app.UseSpa(spa =>
        {
        }); //Handles all requests by returning the default page (wwwroot) for the Single Page Application (SPA).
        return app;
    }
    
    /// <summary>
    /// Adds health check endpoints
    /// </summary>
    /// <param name="applicationBuilder"><see cref="IApplicationBuilder"/></param>
    /// <returns></returns>
    private static IApplicationBuilder MapHealthcheckEndpoints(this IApplicationBuilder applicationBuilder)
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