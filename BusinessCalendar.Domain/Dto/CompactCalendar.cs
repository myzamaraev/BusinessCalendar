using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Extensions;

namespace BusinessCalendar.Domain.Dto
{
    public class CompactCalendar
    {
        public CalendarId Id { get; set; }
        public List<DateTime> Holidays { get; set; } = new List<DateTime>();
        public List<DateTime> ExtraWorkDays { get; set; } = new List<DateTime>();

        public CompactCalendar(Calendar calendar)
        {
            Id = calendar.Id;
            foreach (var date in calendar.Dates)
            {
                switch (date.IsWorkday)
                {
                    case true when date.Date.IsWeekend():
                        ExtraWorkDays.Add(date.Date);
                        break;
                    case false when !date.Date.IsWeekend(): 
                        Holidays.Add(date.Date);
                        break;
                }
            }
        }

        public bool IsWorkDay(DateTime date)
        {
            var isNotWeekend = !date.IsWeekend() || ExtraWorkDays.Any(x => x.DateEquals(date));
            return isNotWeekend && !Holidays.Any(x => x.DateEquals(date));
        }
    }
}