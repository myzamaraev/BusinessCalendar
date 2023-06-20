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

        private List<DateOnly> _holidays = new List<DateOnly>();
        public List<DateOnly> Holidays { get; private set; } = new List<DateOnly>();
        public List<DateOnly> ExtraWorkDays { get; private set; } = new List<DateOnly>();

        public CompactCalendar(CalendarId id, List<DateOnly> holidays, List<DateOnly> extraWorkDays)
        {
            Id = id;
            _holidays = holidays;
            ExtraWorkDays = extraWorkDays;
        }

        public CompactCalendar(Calendar calendar)
        {
            Id = calendar.Id;
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
            var isNotWeekend = !date.IsWeekend() || ExtraWorkDays.Any(x => x.Equals(date));
            return isNotWeekend && !Holidays.Any(x => x.Equals(date));
        }
    }
}