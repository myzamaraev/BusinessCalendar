using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
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