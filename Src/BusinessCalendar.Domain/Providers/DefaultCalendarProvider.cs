using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Extensions;

namespace BusinessCalendar.Domain.Providers;

/// <summary>
/// Provider of default year layouts
/// </summary>
public static class DefaultCalendarProvider
{
    /// <summary>
    /// Provides a collection of CalendarDate items inside a year
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public static IEnumerable<CalendarDate> DefaultCalendar(int year)
    {
        year.CheckYearValidity();
            
        var date = new DateOnly(year,1,1);
        while (date.Year == year)
        {
            yield return new CalendarDate { 
                Date = date, 
                IsWorkday = !date.IsWeekend()
            };

            date = date.AddDays(1);
        }
    }
}