using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.Domain.Services
{
    public interface ICalendarManagementService
    {
        public Task AddCalendarIdentifierAsync(CalendarType type, string key);

        public Task<List<CalendarIdentifier>> GetCalendarIdentifiersAsync(int page, int pageSize);
        
        public Task<Calendar> GetCalendarAsync(CalendarType type, string key, int year);

        public Task<CompactCalendar> GetCompactCalendarAsync(CalendarType type, string key, int year);

        public Task SaveCalendarAsync(Calendar calendar);
        
        public Task SaveCalendarAsync(CompactCalendar compactCalendar);
    }
}