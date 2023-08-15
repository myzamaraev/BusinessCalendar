using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Extensions;

namespace BusinessCalendar.Domain.Mappers;

public class CalendarMapper : ICalendarMapper
{
    public Calendar Map(SaveCalendarRequest saveCalendarRequest)
    {
        var calendar = new Calendar(
            saveCalendarRequest.Type, 
            saveCalendarRequest.Key, 
            saveCalendarRequest.Year, 
            saveCalendarRequest.Dates);
        
        return calendar;
    }
    
    public CompactCalendar MapToCompact(SaveCalendarRequest saveCalendarRequest)
    {
        var calendar = Map(saveCalendarRequest);
        return calendar.ToCompact();
    }

    public CompactCalendar MapToCompact(Calendar calendar)
    {
        return calendar.ToCompact();
    }

    public CompactCalendar MapToCompact(SaveCompactCalendarRequest request)
    {
        return new CompactCalendar(
            new CalendarId 
            { 
                Type = request.Type, 
                Key = request.Key, 
                Year = request.Year 
            },
            request.Holidays,
            request.ExtraWorkDays);
    }
}