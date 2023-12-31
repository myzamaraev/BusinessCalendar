using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto;

/// <summary>
/// represents CalendarIdentifier as immutable record
/// </summary>
public record CalendarIdentifier
{
    public string Id { get; }
    public CalendarType Type { get; }
    public string Key { get; }
    
    public CalendarIdentifier(CalendarType type, string key)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        
        Type = type;
        Key = key;
        Id = $"{type}_{key}";
    }

    /// <summary>
    /// private parameterless constructor required for ORMs
    /// </summary>
    private CalendarIdentifier()
    {
        Id = String.Empty;
        Key = String.Empty;
    }
}