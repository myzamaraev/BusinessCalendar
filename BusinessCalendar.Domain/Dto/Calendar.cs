using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Providers;

namespace BusinessCalendar.Domain.Dto
{
    public class Calendar
    {
        public CalendarId Id { get; set; }
        public List<CalendarDate> Dates { get; set; } = new List<CalendarDate>();

        public Calendar(CalendarType type, string key, int year)
        {
            Id = new CalendarId {
                Type = type,
                Key = key,
                Year = year
            };

            Dates.AddRange(DefaultCalendarProvider.DefaultCalendar(year).ToList());
        }

        public Calendar(CompactCalendar compactCalendar)
        {
            Id = compactCalendar.Id;
            Dates = DefaultCalendarProvider.DefaultCalendar(compactCalendar.Id.Year).ToList();
            Dates.ForEach(x => x.IsWorkday = compactCalendar.IsWorkDay(x.Date));
        }
    }
}