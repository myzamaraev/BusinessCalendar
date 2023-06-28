using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Options;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.MongoDb.StorageServices
{
    public class CalendarIdentifierStorageService : ICalendarIdentifierStorageService
    {
        private readonly IMongoCollection<CalendarIdentifier> _collection;

        public CalendarIdentifierStorageService(IOptions<MongoDBSettings> mongoDbSettings)
        {
            //todo: make client singletone?
            var client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _collection = database.GetCollection<CalendarIdentifier>("CalendarIdentifier");
        }

        public async Task InsertAsync(CalendarIdentifier calendarIdentifier)
        {
            try {
                await _collection.InsertOneAsync(calendarIdentifier);
            }
            catch (MongoWriteException e) when(e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                //todo: turn into client exception according to problem details pattern
                throw new Exception("Calendar identifier already exists");
            }
        }

        public Task<List<CalendarIdentifier>> GetAllAsync(int page, int pageSize)
        {
            return _collection.AsQueryable()
                .OrderBy(x => x.Id)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}