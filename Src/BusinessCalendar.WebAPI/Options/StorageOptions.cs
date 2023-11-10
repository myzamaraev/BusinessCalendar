namespace BusinessCalendar.WebAPI.Options;

public class StorageOptions
{
    public const string Section = "Storage";
    
    /// <summary>
    /// Type of storage layer to use
    /// </summary>
    public string DatabaseType { get; set; } = "MongoDb";
    
    /// <summary>
    /// Defines amount of time after which database connection is treated as unhealthy
    /// </summary>
    public int HealthTimeoutSeconds { get; set; } = 3;
    
    /// <summary>
    /// Enables automatic migrations for databases with schema if any
    /// </summary>
    public bool AllowAutoMigrations { get; set; }
}