using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto.Requests
{
    public class AddCalendarIdentifierRequest
    {
        public CalendarType Type { get; set; }
        public string Key { get; set; }
    }
}