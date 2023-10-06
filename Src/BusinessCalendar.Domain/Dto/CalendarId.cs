using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto;

/// <summary>
/// Represents CalendarId as an immutable record
/// not a struct to prevent a possibility of initializing Key property with default null value with parameterless constructor
/// </summary>
/// <param name="Type"></param>
/// <param name="Key"></param>
/// <param name="Year"></param>
public record CalendarId(CalendarType Type, string Key, int Year)
{
    /// <summary>
    /// parameterless constructor to allow initializers, but ensuring Key always not null
    /// </summary>
    public CalendarId() 
        : this(default, string.Empty, default) 
    {
    }
}