using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.MongoDb
{
    public static class MongoClassMapper
    {
        public static void Register()
        {
            BsonSerializer.TryRegisterSerializer(typeof(DateOnly), new DateOnlySerializer());

            BsonClassMap.TryRegisterClassMap<CompactCalendar>(cm =>
                {
                    cm.AutoMap();
                });
        }
    }

    /// <summary>
    /// Serializes DateOnly as ISODate (BsonType.DateTime) — the v3 default.
    /// Deserializes both the legacy v2 document format { Year, Month, Day }
    /// and the current ISODate format for backward compatibility.
    /// Note: MongoDB.Bson.Serialization.Serializers.DateOnlySerializer is sealed,
    /// so composition via SerializerBase is used instead of inheritance.
    /// </summary>
    internal class DateOnlySerializer : SerializerBase<DateOnly>
    {
        private static readonly MongoDB.Bson.Serialization.Serializers.DateOnlySerializer _standard = new();

        public override DateOnly Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Document)
            {
                int year = 0, month = 0, day = 0;
                context.Reader.ReadStartDocument();
                while (context.Reader.ReadBsonType() != BsonType.EndOfDocument)
                {
                    var name = context.Reader.ReadName(Utf8NameDecoder.Instance);
                    switch (name)
                    {
                        case "Year": year = context.Reader.ReadInt32(); break;
                        case "Month": month = context.Reader.ReadInt32(); break;
                        case "Day": day = context.Reader.ReadInt32(); break;
                        default: context.Reader.SkipValue(); break;
                    }
                }
                context.Reader.ReadEndDocument();
                return new DateOnly(year, month, day);
            }

            return _standard.Deserialize(context, args);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateOnly value)
            => _standard.Serialize(context, args, value);
    }
}
