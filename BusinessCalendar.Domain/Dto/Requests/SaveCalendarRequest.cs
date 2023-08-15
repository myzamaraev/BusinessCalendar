using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto.Requests
{
    public class SaveCalendarRequest
    {
        public CalendarType Type { get; set; }
        public string Key { get; set; }
        public int Year { get; set; }
        public List<CalendarDate> Dates { get;  set;} = new ();
    }
}