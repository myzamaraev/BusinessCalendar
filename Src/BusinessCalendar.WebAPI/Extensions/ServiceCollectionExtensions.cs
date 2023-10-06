using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.MongoDb.Extensions;
using BusinessCalendar.MongoDb.Options;
using BusinessCalendar.WebAPI.Constants;
using BusinessCalendar.WebAPI.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BusinessCalendar.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds BusinessCalendar domain services, storage and health checks
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.InitSettings(configuration);
        services.AddMongoDbStorage();
        services.AddBusinessCalendarDomain();
        services.AddHealthChecks(configuration);
        return services;
    }

    private static IServiceCollection InitSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions()
            .Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.Section));

        services.AddOptions()
            .Configure<AuthOptions>(configuration.GetSection(AuthOptions.Section));

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection(MongoDbOptions.Section).Get<MongoDbOptions>();
        if (mongoDbSettings == null)
        {
            throw new Exception($"No proper configuration section found for key {MongoDbOptions.Section}");
        }

        services.AddHealthChecks().AddMongoDb(
            mongoDbSettings.ConnectionUri,
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { HealthConstants.LiveTag, HealthConstants.ReadyTag },
            timeout: TimeSpan.FromSeconds(mongoDbSettings.HealthTimeoutSeconds ?? 3));
        return services;
    }

    
}