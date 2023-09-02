using System;
using System.Threading.Tasks;
using BusinessCalendar.Client.Dto;

namespace BusinessCalendar.Client
{
    public interface IBusinessCalendarClient
    {
        /// <summary>
        /// Returns the calendar for provided identifier and year
        /// </summary>
        /// <param name="identifier">Calendar identifier, find it on the calendar settings page</param>
        /// <param name="year"></param>
        /// <returns></returns>
        Task<CalendarModel> GetCalendarAsync(string identifier, int year);
        
        /// <summary>
        /// Returns the information about particular calendar date
        /// </summary>
        /// <param name="identifier">Calendar identifier, find it on the calendar settings page</param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<CalendarDateModel> GetDateAsync(string identifier, DateTime date);
    }
}
