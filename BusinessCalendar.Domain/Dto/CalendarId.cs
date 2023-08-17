using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto
{
    public class CalendarId
    {
        public CalendarType Type { get; set; }
        public string Key { get; set; }
        public int Year { get; set; }

        public CalendarId()
        {
            
        }

        public CalendarId(CalendarId calendarId)
        {
            Type = calendarId.Type;
            Key = calendarId.Key;
            Year = calendarId.Year;
        }
    }
}