using BusinessCalendar.Domain.Extensions;

namespace BusinessCalendar.Domain.Dto;

/// <summary>
/// Represents compact version of calendar
/// </summary>
/// <remarks>
/// Any derived type should be truly immutable,
/// In case it isn't possible all persistent properties should be rewritten to implement at least init setters  to avoid db serialization errors
/// </remarks>
public class CompactCalendar
{
    public CalendarId Id { get; }
    public List<DateOnly> Holidays { get; } = new ();
    public List<DateOnly> ExtraWorkDays { get; } = new ();

    public bool IsDefault => !Holidays.Any() && !ExtraWorkDays.Any();

    /// <summary>
    /// Creates CompactCalendar with default workdays/weekends;
    /// </summary>
    /// <param name="id"></param>
    public CompactCalendar(CalendarId id)
    {
        Id = id;
    }

    /// <summary>
    /// Creates CompactCalendar with specified Holidays and extraworkdays
    /// </summary>
    /// <param name="id"></param>
    /// <param name="holidays"></param>
    /// <param name="extraWorkDays"></param>
    public CompactCalendar(CalendarId id, List<DateOnly> holidays, List<DateOnly> extraWorkDays)
    {
        Id = id;
        Holidays = holidays;
        ExtraWorkDays = extraWorkDays;
    }

    /// <summary>
    /// Creates CompactCalendar from full Calendar
    /// </summary>
    /// <param name="calendar"></param>
    public CompactCalendar(Calendar calendar)
    {
        Id = calendar.Id;
        foreach (var calendarDate in calendar.Dates)
        {
            switch (calendarDate.IsWorkday)
            {
                case true when calendarDate.Date.IsWeekend():
                    ExtraWorkDays.Add(calendarDate.Date);
                    break;
                case false when !calendarDate.Date.IsWeekend(): 
                    Holidays.Add(calendarDate.Date);
                    break;
            }
        }
    }

    /// <summary>
    /// checks particular date for workday according to calendar
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public bool IsWorkDay(DateOnly date)
    {
        var isDayOff = date.IsWeekend() || Holidays.Any(holiday => holiday.Equals(date));
        var isExtraWorkDay = ExtraWorkDays.Any(extraWorkDay => extraWorkDay.Equals(date));
        
        return isExtraWorkDay || !isDayOff;
    }
    
    /// <summary>
    /// private parameterless constructor required for ORMs
    /// </summary>
    private CompactCalendar()
    {
        Id = new CalendarId();
    }
}