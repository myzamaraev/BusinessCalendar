using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.Domain.Extensions;

public static class CalendarExtensions
{
    /// <summary>
    /// Converts full calendar to it's compact version
    /// </summary>
    /// <param name="calendar"></param>
    /// <returns></returns>
    public static CompactCalendar ToCompact(this Calendar calendar)
    {
        return new CompactCalendar(calendar);
    }
}