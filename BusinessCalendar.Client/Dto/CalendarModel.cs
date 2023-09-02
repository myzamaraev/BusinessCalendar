using System;
using System.Collections.Generic;
using System.Linq;
using BusinessCalendar.Client.Extensions;

namespace BusinessCalendar.Client.Dto
{
    public class CalendarModel
    {
        /// <summary>
        /// Calendar Type 
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Calendar Key
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// Calendar year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// The list of holidays
        /// </summary>
        public List<DateTime> Holidays { get; set; } = new List<DateTime>();

        /// <summary>
        /// The list of extra work days
        /// </summary>
        public List<DateTime> ExtraWorkDays { get; set; } = new List<DateTime>();
        
        
        /// <summary>
        /// Checks the date is workday according to calendar
        /// </summary>
        /// <param name="date"></param>
        /// <returns>True when workday, false when day off</returns>
        public virtual bool IsWorkday(DateTime date)
        {
            if (date.Year != Year)
            {
                throw new ArgumentOutOfRangeException(nameof(date), $"Date {date:yyyy-MM-dd} is out of the year {Year}");
            }
            
            var isDayOff = date.IsWeekend() || Holidays.Any(holiday => holiday.DatePartEquals(date));
            var isExtraWorkDay = ExtraWorkDays.Any(extraWorkDay => extraWorkDay.DatePartEquals(date));
            
            return isExtraWorkDay || !isDayOff;
        }
    }
}