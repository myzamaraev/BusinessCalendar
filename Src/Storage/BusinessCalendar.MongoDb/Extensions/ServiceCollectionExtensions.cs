using Microsoft.Extensions.DependencyInjection;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.MongoDb.StorageServices;
using BusinessCalendar.MongoDb.Options;
using HealthChecks.MongoDb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace BusinessCalendar.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds storage services to ServiceCollection and registers MongoClassMapper
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMongoDbStorage(this IServiceCollection services)
    {
        services.InitMongoClient();
        services.AddSingleton<ICalendarStorageService, CalendarStorageService>();
        services.AddSingleton<ICalendarIdentifierStorageService, CalendarIdentifierStorageService>();
        
        MongoClassMapper.Register();
        
        return services;
    }
    
    /// <summary>
    /// Add a health check for MongoDb database
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/></param>
    /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'mongodb' will be used for the name.</param>
    /// <param name="failureStatus">
    /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
    /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
    /// </param>
    /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
    /// <param name="timeout">An optional <see cref="TimeSpan"/> representing the timeout of the check.</param>
    public static void AddMongoDbHealthCheck(
        this IServiceCollection services,
        string name = "mongodb",
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        services.AddHealthChecks()
            .AddMongoDb(sp => sp.GetRequiredService<IOptions<MongoDbOptions>>().Value.ConnectionUri,
                name: name,
                failureStatus: failureStatus,
                tags: tags,
                timeout: timeout);
    }

    /// <summary>
    /// Adds mongo Client
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection InitMongoClient(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(
            sp =>
            {
                var dbSettings = sp.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                var mongoUrl = new MongoUrl(dbSettings.ConnectionUri);
                return new MongoClient(mongoUrl);
            });

        return services;
    }
}