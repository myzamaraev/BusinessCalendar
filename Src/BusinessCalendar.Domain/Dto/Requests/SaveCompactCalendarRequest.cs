using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto.Requests
{
    public class SaveCompactCalendarRequest
    {
        public CalendarType Type { get; set; }
        public string Key { get; set; } = string.Empty;
        public int Year { get; set; }

         public List<DateOnly> Holidays { get; set;  } = new();
        public List<DateOnly> ExtraWorkDays { get; set;  } = new();
    }
}