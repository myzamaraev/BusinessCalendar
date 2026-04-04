using System;
using System.Threading.Tasks;

namespace BusinessCalendar.Client.Providers;

/// <summary>
/// WorkdayProvider interface
/// </summary>
public interface IWorkdayProvider
{
    /// <summary>
    /// Checks the date for workday according to the calendar with specified identifier
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="date">
    /// DateTime object with appropriate timezone applied. Method uses date part as is and doesn't care about the time zone.
    /// </param>
    /// <returns>True when workday, false otherwise</returns>
    /// <throws name="ArgumentOutOfRangeException">The identifier has invalid format</throws>
    Task<bool> IsWorkday(string identifier, DateTime date);

    /// <summary>
    /// Finds the next workday at a given distance from the specified date.
    /// </summary>
    /// <param name="identifier">Calendar identifier</param>
    /// <param name="date">The base date to calculate from. Method uses date part as is and doesn't care about the time zone.</param>
    /// <param name="daysOffset">Number of days to offset from the base date</param>
    /// <param name="workdaysOnly">
    /// When <c>false</c>: adds <paramref name="daysOffset"/> calendar days to <paramref name="date"/>, then returns the first workday on or after that result.<br/>
    /// When <c>true</c>: steps forward exactly <paramref name="daysOffset"/> workdays from <paramref name="date"/>, skipping weekends and holidays.
    /// </param>
    /// <returns>The resulting workday as DateTime</returns>
    /// <exception cref="InvalidOperationException">Thrown when no workday is found within 365 days</exception>
    Task<DateTime> GetNextWorkday(string identifier, DateTime date, int daysOffset, bool workdaysOnly);
}