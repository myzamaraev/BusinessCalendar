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
}