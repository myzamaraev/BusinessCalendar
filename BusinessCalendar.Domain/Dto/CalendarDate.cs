using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BusinessCalendar.Domain.Dto
{
    //todo: value type breaks updates in foreach 
    public class CalendarDate
    {
        public DateOnly Date { get; set; }

        public bool IsWorkday { get; set; }
    }
}