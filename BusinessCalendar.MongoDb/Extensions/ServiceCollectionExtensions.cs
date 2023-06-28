using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.MongoDb.StorageServices;
using BusinessCalendar.MongoDb;

namespace BusinessCalendar.MongoDb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMongoDbStorage(this IServiceCollection services, IConfigurationSection mongoDBConfiguration)
        {
            services.Configure<MongoDBSettings>(mongoDBConfiguration);
            services.AddSingleton<ICalendarStorageService, CalendarStorageService>();
            services.AddSingleton<ICalendarIdentifierStorageService, CalendarIdentifierStorageService>();
        }
    }
}