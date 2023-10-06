namespace BusinessCalendar.Domain.Extensions;

public static class DateOnlyExtensions
{
    /// <summary>
    /// checks a date for weekend
    /// </summary>
    /// <param name="date"></param>
    /// <returns>true if saturday or sunday, false otherwise</returns>
    public static bool IsWeekend(this DateOnly date)
    {
        return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
    }
}