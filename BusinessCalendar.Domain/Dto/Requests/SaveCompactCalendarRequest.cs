using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto.Requests
{
    public class SaveCompactCalendarRequest
    {
        public CalendarType Type { get; set; }
        public string Key { get; set; }
        public int Year { get; set; }

         public List<DateOnly> Holidays { get; } = new List<DateOnly>();
        public List<DateOnly> ExtraWorkDays { get; } = new List<DateOnly>();
    }
}