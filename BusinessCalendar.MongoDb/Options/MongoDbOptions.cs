namespace BusinessCalendar.MongoDb.Options;

public class MongoDbOptions
{
    public const string Section = "MongoDB";
    public string ConnectionUri { get; set; }
    public string DatabaseName { get; set; }
    public int? HealthTimeoutSeconds { get; set; }
}