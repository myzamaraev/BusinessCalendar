using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.MongoDb.Extensions;
using BusinessCalendar.MongoDb.Options;
using BusinessCalendar.Postgres.Extensions;
using BusinessCalendar.Postgres.Options;
using BusinessCalendar.WebAPI.Constants;
using BusinessCalendar.WebAPI.Options;
using BusinessCalendar.WebAPI.Swagger;
using Microsoft.OpenApi.Models;

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
        services.AddStorage(configuration);
        services.AddBusinessCalendarDomain();
        return services;
    }

    /// <summary>
    /// Adds Swagger/OpenAPI documentation
    /// </summary>
    /// <param name="services"></param>
    /// <param name="publicApiOnly">show only public API methods (exclude BFF etc.)</param>
    /// <returns></returns>
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, bool publicApiOnly = true)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.MapType<DateOnly>(() => new OpenApiSchema { Type = nameof(String).ToLower(), Format = "date" });
            if (publicApiOnly)
            {
                c.DocumentFilter<PublicApiDocumentFilter>();
            }
        });

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