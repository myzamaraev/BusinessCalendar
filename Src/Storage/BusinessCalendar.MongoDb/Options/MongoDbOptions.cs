namespace BusinessCalendar.MongoDb.Options;

/// <summary>
/// MongoDbOptions to configure MongoClient and database
/// </summary>
public class MongoDbOptions
{
    public const string Section = "MongoDB";
    public string ConnectionUri { get; set; } = "mongodb://localhost:27017";
    public string DatabaseName { get; set; } = "BusinessCalendar";
    public int? HealthTimeoutSeconds { get; set; } 
}