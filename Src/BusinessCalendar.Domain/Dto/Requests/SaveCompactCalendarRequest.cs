using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto.Requests;

/// <summary>
/// Contract to save CompactCalendar to DB
/// </summary>
public class SaveCompactCalendarRequest
{
    /// <summary>
    /// Calendar Type 
    /// </summary>
    public CalendarType Type { get; set; }
        
    /// <summary>
    /// Calendar Key 
    /// </summary>
    public string Key { get; set; } = string.Empty;
        
    /// <summary>
    /// Calendar Year 
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// The list of holidays
    /// </summary>
    public List<DateOnly> Holidays { get; set;  } = new();
        
    /// <summary>
    /// The list of extra work days
    /// </summary>
    public List<DateOnly> ExtraWorkDays { get; set;  } = new();
}