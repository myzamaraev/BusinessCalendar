using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.WebAPI.ViewModels
{
    public class AddCalendarRequest
    {
        public CalendarType Type { get; set; }
        public string Key { get; set; }
    }
}