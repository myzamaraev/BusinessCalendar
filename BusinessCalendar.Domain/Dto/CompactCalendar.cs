using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto
{
    public class CompactCalendar
    {
        public CalendarId Id { get; set; }
        public List<DateOnly> Holidays { get; private set; } = new (); //todo: private set for the sake of Mongo driver
        public List<DateOnly> ExtraWorkDays { get; private set; } = new ();

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
            Id = new CalendarId(calendar.Id);
            
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

        public bool IsWorkDay(DateOnly date)
        {
            var isDayOff = date.IsWeekend() || Holidays.Any(holiday => holiday.Equals(date));
            var isExtraWorkDay = ExtraWorkDays.Any(extraWorkDay => extraWorkDay.Equals(date));
            
            return isExtraWorkDay || !isDayOff;
        }
        
        
    }
}