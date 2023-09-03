using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;

namespace BusinessCalendar.Domain.Services
{
    public interface ICalendarManagementService
    {
        public Task<Calendar> GetCalendarAsync(CalendarId calendarId);

        public Task<CompactCalendar> GetCompactCalendarAsync(CalendarId calendarId);

        public Task SaveCalendarAsync(SaveCalendarRequest request);
        
        public Task SaveCompactCalendarAsync(SaveCompactCalendarRequest request);
        
    }
}