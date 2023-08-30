using System;
using System.Collections.Generic;

namespace BusinessCalendar.Contracts.ApiContracts
{
    public class GetCalendarResponse
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public int Year { get; set; }
        public List<DateTime> Holidays { get; set; }
        public List<DateTime> ExtraWorkDays { get; set; }
    }
}