using System;

namespace BusinessCalendar.Client.Extensions;

internal static class DateTimeExtensions
{
    /// <summary>
    /// Compares two dates by year month and day.
    /// Doesn't take time zone into account.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    internal static bool DatePartEquals(this DateTime first, DateTime second)
    {
        return first.Year == second.Year
               && first.Month == second.Month
               && first.Day == second.Day;
    }
        
    /// <summary>
    /// Checks the date is Saturday or Sunday
    /// </summary>
    /// <param name="date"></param>
    /// <returns>True if saturday or sunday, False otherwise</returns>
    internal static bool IsWeekend(this DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
    }
}