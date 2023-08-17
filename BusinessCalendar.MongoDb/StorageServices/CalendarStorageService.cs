using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace BusinessCalendar.MongoDb.StorageServices
{
    public class CalendarStorageService : ICalendarStorageService
    {
        private readonly IMongoCollection<CompactCalendar> _calendarCollection;
        public CalendarStorageService(IOptions<MongoDBSettings> mongoDbSettings)
        {
            //todo: make client singletone?
            var client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _calendarCollection = database.GetCollection<CompactCalendar>("Calendar");
        }

        public async Task Upsert(CompactCalendar compactCalendar)
        {
            //todo: implement equality operator for CalendarId
            var result = await _calendarCollection.ReplaceOneAsync(
                    x => x.Id == compactCalendar.Id,
                    compactCalendar,
                    new ReplaceOptions { IsUpsert = true });
            
            if (result.IsAcknowledged == false
                || (result.MatchedCount == 0 && result.UpsertedId == null))
            {
                throw new Exception("Error saving calendar to DB");
            }
        }
        public async Task<CompactCalendar> FindOne(CalendarId id)
        {
            //todo: eqality operator + CalendarId as input?
            var result = await _calendarCollection.FindAsync(x => x.Id.Type == id.Type 
                && x.Id.Key == id.Key
                && x.Id.Year == id.Year);

            return result.SingleOrDefault();
        }

        public async Task DeleteMany(CalendarType type, string key)
        {
            var result = await _calendarCollection.DeleteManyAsync(x => x.Id.Type == type && x.Id.Key == key);
        }
    }
}