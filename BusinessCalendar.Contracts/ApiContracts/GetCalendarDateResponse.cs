using System;

namespace BusinessCalendar.Contracts.ApiContracts
{
    public class GetCalendarDateResponse
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public int Year { get; set; }
        public DateTime Date { get; set; }
        public bool IsWorkday { get; set; }
    }
}