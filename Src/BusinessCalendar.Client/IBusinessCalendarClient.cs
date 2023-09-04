using System;
using System.Threading.Tasks;
using BusinessCalendar.Client.Dto;

namespace BusinessCalendar.Client
{
    /// <summary>
    /// Client interface to access BusinessCalendar API
    /// </summary>
    public interface IBusinessCalendarClient
    {
        /// <summary>
        /// Returns the calendar for provided identifier and year
        /// </summary>
        /// <param name="identifier">Calendar identifier, find it on the calendar settings page</param>
        /// <param name="year">An integer value representing year</param>
        /// <returns name="CalendarDateModel"></returns>
        Task<CalendarModel> GetCalendarAsync(string identifier, int year);
        
        /// <summary>
        /// Returns the information about particular calendar date
        /// </summary>
        /// <param name="identifier">Calendar identifier, find it on the calendar settings page</param>
        /// <param name="date"></param>
        /// <returns name="CalendarDateModel"></returns>
        Task<CalendarDateModel> GetDateAsync(string identifier, DateTime date);
    }
}
