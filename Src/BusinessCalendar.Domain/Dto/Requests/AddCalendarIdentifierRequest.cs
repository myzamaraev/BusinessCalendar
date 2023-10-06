using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto.Requests;

/// <summary>
/// Contract to add new CalendarIdentifier
/// </summary>
public class AddCalendarIdentifierRequest
{
    /// <summary>
    /// Calendar Type 
    /// </summary>
    public CalendarType Type { get; set; }
    
    /// <summary>
    /// Calendar Key 
    /// </summary>
    public string Key { get; set; } = string.Empty;
}