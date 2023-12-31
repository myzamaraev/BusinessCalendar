using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Options;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Exceptions;
using BusinessCalendar.MongoDb.Options;

namespace BusinessCalendar.MongoDb.StorageServices
{
    public class CalendarIdentifierStorageService : ICalendarIdentifierStorageService
    {
        private readonly IMongoCollection<CalendarIdentifier> _collection;

        public CalendarIdentifierStorageService(IMongoClient mongoClient, IOptions<MongoDbOptions> mongoDbSettings)
        {
            var database = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _collection = database.GetCollection<CalendarIdentifier>("CalendarIdentifier");
        }

        public async Task InsertAsync(CalendarIdentifier calendarIdentifier, CancellationToken cancellationToken = default)
        {
            try {
                await _collection.InsertOneAsync(calendarIdentifier, cancellationToken: cancellationToken);
            }
            catch (MongoWriteException e) when(e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new DuplicateKeyClientException(nameof(calendarIdentifier), calendarIdentifier.Id);
            }
        }

        public async Task<CalendarIdentifier?> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _collection.AsQueryable().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
             var result = await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken: cancellationToken);
             
             if (result.IsAcknowledged == false
                 || result.DeletedCount == 0)
             {
                 throw new DocumentNotFoundClientException(nameof(CalendarIdentifier), id);
             }
        }

        public Task<List<CalendarIdentifier>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        {
            return _collection.AsQueryable()
                .OrderBy(x => x.Type)
                .ThenBy(x => x.Key)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}