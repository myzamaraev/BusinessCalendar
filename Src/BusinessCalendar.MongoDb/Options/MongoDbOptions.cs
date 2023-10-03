namespace BusinessCalendar.MongoDb.Options;

public class MongoDbOptions
{
    public const string Section = "MongoDB";
    public string ConnectionUri { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "BusinessCalendar";
    public int? HealthTimeoutSeconds { get; set; } 
}