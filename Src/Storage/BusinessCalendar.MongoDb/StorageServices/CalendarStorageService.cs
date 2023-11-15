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

            EnsureCompositeIdIndex(_calendarCollection);
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
        public async Task<CompactCalendar?> FindOneAsync(CalendarId id, CancellationToken cancellationToken = default)
        {
            var cursor = await _calendarCollection.FindAsync(x => x.Id == id, cancellationToken: cancellationToken);
            return cursor.SingleOrDefault(cancellationToken: cancellationToken);
        }

        public async Task DeleteManyAsync(CalendarType type, string key, CancellationToken cancellationToken = default)
        {
            await _calendarCollection.DeleteManyAsync(x => x.Id.Type == type && x.Id.Key == key, cancellationToken: cancellationToken);
        }
        
        /// <summary>
        /// creates a unique index by _id fields if not exists
        /// PK uses hash of _id object, so unique constraint can be violated by changing the order of fields
        /// this unique index prevents violations and covers search queries
        /// </summary>
        /// <param name="calendarCollection"></param>
        private static void EnsureCompositeIdIndex(IMongoCollection<CompactCalendar> calendarCollection)
        {
            var indexKeys = Builders<CompactCalendar>.IndexKeys
                .Ascending(x => x.Id.Type)
                .Ascending(x => x.Id.Key)
                .Ascending(x => x.Id.Year);
            
            var indexModel = new CreateIndexModel<CompactCalendar>(indexKeys, new CreateIndexOptions
            {
                Unique = true
            });
            calendarCollection.Indexes.CreateOne(indexModel);
        }
    }
}