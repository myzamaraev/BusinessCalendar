namespace BusinessCalendar.Postgres.Options;

/// <summary>
/// Postgres options to configure SQL connection
/// </summary>
public class PostgresOptions
{
    public const string Section = "Postgres";
    public string ConnectionString { get; set; } = string.Empty;
    public bool UseAutoMigrations { get; set; }
}