using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;

namespace BusinessCalendar.Domain.Mappers;

public interface ICalendarMapper
{
    /// <summary>
    /// Maps CompactCalendar to full Calendar
    /// </summary>
    /// <param name="compactCalendar"></param>
    /// <returns>Calendar</returns>
    public Calendar Map(CompactCalendar compactCalendar);
    
    /// <summary>
    /// Maps SaveCalendarRequest to CompactCalendar
    /// </summary>
    /// <param name="saveCalendarRequest"></param>
    /// <returns>CompactCalendar</returns>
    public CompactCalendar MapToCompact(SaveCalendarRequest saveCalendarRequest);

    /// <summary>
    /// Maps full Calendar to CompactCalendar
    /// </summary>
    /// <param name="calendar"></param>
    /// <returns>CompactCalendar</returns>
    public CompactCalendar MapToCompact(Calendar calendar);

    /// <summary>
    /// Maps SaveCompactCalendarRequest to CompactCalendar
    /// </summary>
    /// <param name="saveCompactCalendarRequest"></param>
    /// <returns>CompactCalendar</returns>
    public CompactCalendar MapToCompact(SaveCompactCalendarRequest saveCompactCalendarRequest);
}