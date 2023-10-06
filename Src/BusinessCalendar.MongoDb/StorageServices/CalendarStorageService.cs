using Microsoft.Extensions.Options;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.MongoDb.Options;
using MongoDB.Driver;

namespace BusinessCalendar.MongoDb.StorageServices
{
    public class CalendarStorageService : ICalendarStorageService
    {
        private readonly IMongoCollection<CompactCalendar> _calendarCollection;
        public CalendarStorageService(IMongoClient mongoClient, IOptions<MongoDbOptions> mongoDbSettings)
        {
            var database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _calendarCollection = database.GetCollection<CompactCalendar>("Calendar");
        }

        public async Task UpsertAsync(CompactCalendar compactCalendar, CancellationToken cancellationToken = default)
        {
            var result = await _calendarCollection.ReplaceOneAsync(
                    x => x.Id == compactCalendar.Id,
                    compactCalendar,
                    new ReplaceOptions { IsUpsert = true }, 
                    cancellationToken: cancellationToken);
            
            if (result.IsAcknowledged == false
                || (result.MatchedCount == 0 && result.UpsertedId == null))
            {
                throw new Exception("Error saving calendar to DB");
            }
        }
        public async Task<CompactCalendar> FindOneAsync(CalendarId id, CancellationToken cancellationToken = default)
        {
            var cursor = await _calendarCollection.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
            return cursor.SingleOrDefault(cancellationToken: cancellationToken);
        }

        public async Task DeleteManyAsync(CalendarType type, string key, CancellationToken cancellationToken = default)
        {
            await _calendarCollection.DeleteManyAsync(x => x.Id.Type == type && x.Id.Key == key, cancellationToken: cancellationToken);
        }
    }
}