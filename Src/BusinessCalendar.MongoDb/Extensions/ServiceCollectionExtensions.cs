using Microsoft.Extensions.DependencyInjection;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.MongoDb.StorageServices;
using BusinessCalendar.MongoDb.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BusinessCalendar.MongoDb.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDbStorage(this IServiceCollection services)
    {
        services.InitMongoClient();
        services.AddSingleton<ICalendarStorageService, CalendarStorageService>();
        services.AddSingleton<ICalendarIdentifierStorageService, CalendarIdentifierStorageService>();
        
        MongoClassMapper.Register();
        
        return services;
    }

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