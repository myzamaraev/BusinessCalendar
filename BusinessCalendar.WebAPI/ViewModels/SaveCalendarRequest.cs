using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.WebAPI.ViewModels
{
    public class SaveCalendarRequest
    {
        public CalendarType Type { get; set; }
        public string Key { get; set; }
        public int Year { get; set; }
        public List<CalendarDate> Dates { get; set; } = new List<CalendarDate>();
    }
}