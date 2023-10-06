using System;

namespace BusinessCalendar.Client.Dto;

/// <summary>
/// Model to parse GetCalendar response
/// </summary>
public class CalendarDateModel
{
    /// <summary>
    /// Calendar Type 
    /// </summary>
    public string Type { get; set; }
        
    /// <summary>
    /// Calendar Key
    /// </summary>
    public string Key { get; set; }
        
    /// <summary>
    /// Requested date
    /// </summary>
    public DateTime Date { get; set; }
        
    /// <summary>
    /// Workday or not according to requested calendar
    /// </summary>
    public bool IsWorkday { get; set; }
}