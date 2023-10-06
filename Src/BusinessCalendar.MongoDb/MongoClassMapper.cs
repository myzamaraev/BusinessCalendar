using MongoDB.Bson.Serialization;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.MongoDb
{
    public static class MongoClassMapper
    {
        public static void Register()
        {
            BsonClassMap.TryRegisterClassMap<CompactCalendar>(cm =>
                {
                    cm.AutoMap();
                });
        }
    }
}