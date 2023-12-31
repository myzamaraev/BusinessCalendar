using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Providers;

namespace BusinessCalendar.Domain.Mappers;

/// <summary>
/// Implementation of ICalendarMapper
/// </summary>
public class CalendarMapper : ICalendarMapper
{
    public Calendar Map(CompactCalendar compactCalendar)
    {
        var dates = DefaultCalendarProvider.DefaultCalendar(compactCalendar.Id.Year)
            .Select(x => x with { IsWorkday = compactCalendar.IsWorkDay(x.Date) })
            .ToList();

        return new Calendar(compactCalendar.Id, dates);
    }

    public CompactCalendar MapToCompact(SaveCalendarRequest saveCalendarRequest)
    {
        var calendar = new Calendar(
            saveCalendarRequest.Type,
            saveCalendarRequest.Key,
            saveCalendarRequest.Year,
            saveCalendarRequest.Dates);
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