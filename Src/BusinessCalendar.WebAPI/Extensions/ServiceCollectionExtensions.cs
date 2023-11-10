using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.MongoDb.Extensions;
using BusinessCalendar.MongoDb.Options;
using BusinessCalendar.Postgres.Extensions;
using BusinessCalendar.Postgres.Options;
using BusinessCalendar.WebAPI.Constants;
using BusinessCalendar.WebAPI.Options;
using HealthChecks.MongoDb;
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
        services.AddStorage(configuration);
        services.AddBusinessCalendarDomain();
        return services;
    }

    private static IServiceCollection InitSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.Section));

        return services;
    }

    private static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var storageOptions = configuration.GetSection(StorageOptions.Section).Get<StorageOptions>();
        if (storageOptions == null)
        {
            throw new Exception("Storage section wasn't found in app configuration");
        }

        services.AddOptions().Configure<StorageOptions>(configuration.GetSection(StorageOptions.Section));
        
        switch (storageOptions.DatabaseType)
        {
            case DatabaseTypes.MongoDb: 
                services.AddOptions().Configure<MongoDbOptions>(configuration.GetSection(MongoDbOptions.Section));
                services.AddMongoDbStorage();
                services.AddMongoDbHealthCheck(
                    tags: new[] { HealthConstants.LiveTag, HealthConstants.ReadyTag },
                    timeout: TimeSpan.FromSeconds(storageOptions.HealthTimeoutSeconds));
                break;
            case DatabaseTypes.Postgres:
                services.Configure<PostgresOptions>(configuration.GetSection(PostgresOptions.Section));
                services.AddPostgresStorage();
                services.AddPostgresHealthCheck(
                    tags: new[] { HealthConstants.LiveTag, HealthConstants.ReadyTag },
                    timeout: TimeSpan.FromSeconds(storageOptions.HealthTimeoutSeconds));
                break;
            default:
                throw new Exception($"Unknown DatabaseType provided in the configuration. Value: {storageOptions.DatabaseType}");
        }

        return services;
    }
}