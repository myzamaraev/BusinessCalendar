using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Extensions;

namespace BusinessCalendar.Domain.Providers
{
    public static class DefaultCalendarProvider
    {
        public static IEnumerable<CalendarDate> DefaultCalendar(int year)
        {
            var date = new DateOnly(year,1,1);
            while (date.Year == year)
            {
                yield return new CalendarDate { 
                    Date = date, 
                    IsWorkday = !date.IsWeekend()
                };

                date = date.AddDays(1);
            }
        }
    }
}