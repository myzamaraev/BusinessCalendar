using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task InsertAsync(CalendarIdentifier calendarIdentifier)
        {
            try {
                await _collection.InsertOneAsync(calendarIdentifier);
            }
            catch (MongoWriteException e) when(e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new DuplicateKeyClientException("Calendar identifier already exists");
            }
        }

        public Task<CalendarIdentifier> GetAsync(string id)
        {
            return _collection.AsQueryable().SingleAsync(x => x.Id == id);
        }

        public async Task DeleteAsync(string id)
        {
             var result = await _collection.DeleteOneAsync(x => x.Id == id);
             
             if (result.IsAcknowledged == false
                 || result.DeletedCount == 0)
             {
                 throw new Exception($"No identifier with id: {id}");
             }
        }

        public Task<List<CalendarIdentifier>> GetAllAsync(int page, int pageSize)
        {
            return _collection.AsQueryable()
                .OrderBy(x => x.Type)
                .ThenBy(x => x.Key)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}