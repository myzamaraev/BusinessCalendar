using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto.Requests;

/// <summary>
/// Contract to save Calendar to DB
/// </summary>
public class SaveCalendarRequest
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
    /// The List of calendar dates
    /// </summary>
    public List<CalendarDate> Dates { get;  set;} = new ();
}