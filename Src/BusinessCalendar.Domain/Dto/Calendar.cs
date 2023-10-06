using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Providers;

namespace BusinessCalendar.Domain.Dto;

/// <summary>
/// Represents a calendar for a specific year
/// </summary>
public class Calendar
{
    public CalendarId Id { get; }

    public List<CalendarDate> Dates { get; } = new();

    public Calendar(CalendarType type, string key, int year)
        : this(new CalendarId { Type = type, Key = key, Year = year })
    {
    }

    public Calendar(CalendarType type, string key, int year, IEnumerable<CalendarDate> dates)
        : this(new CalendarId { Type = type, Key = key, Year = year }, dates)
    {
    }

    public Calendar(CalendarId id)
    {
        Id = id;
        Dates.AddRange(DefaultCalendarProvider.DefaultCalendar(id.Year).ToList());
    }

    public Calendar(CalendarId id, IEnumerable<CalendarDate> dates)
    {
        Id = id;
        Dates.AddRange(dates);
    }
}