using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto.Requests
{
    public class AddCalendarRequest
    {
        public CalendarType Type { get; set; }
        public string Key { get; set; }
    }
}