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
        public Task<Calendar> GetCalendarAsync(CalendarType type, string key, int year);

        public Task<CompactCalendar> GetCompactCalendarAsync(CalendarType type, string key, int year);

        public Task SaveCalendarAsync(SaveCalendarRequest request);
        
        public Task SaveCalendarAsync(SaveCompactCalendarRequest request);
        
    }
}