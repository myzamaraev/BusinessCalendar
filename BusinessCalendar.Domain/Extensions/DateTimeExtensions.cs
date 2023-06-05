using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsWeekend(this DateTime date)
        {
            return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        }

        public static bool DateEquals(this DateTime first, DateTime second)
        {
            return first.Year == second.Year 
                && first.Month == second.Month
                && first.Day == second.Day;
        }
    }
}