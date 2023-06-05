using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCalendar.Domain.Dto
{
    public class CalendarDate
    {
        public DateTime Date { get; set; }

        public bool IsWorkday { get; set; }
    }
}