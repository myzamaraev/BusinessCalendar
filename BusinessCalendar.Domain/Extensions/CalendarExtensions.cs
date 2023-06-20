using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.Domain.Extensions
{
    public static class CalendarExtensions
    {
        public static CompactCalendar ToCompact(this Calendar calendar)
        {
            return new CompactCalendar(calendar);
        }
    }
}