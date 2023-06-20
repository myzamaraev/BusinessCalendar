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
        public Task<Calendar> GetCalendar(CalendarType type, string key, int year);

        public Task<CompactCalendar> GetCompactCalendarAsync(CalendarType type, string key, int year);

        public Task SaveCalendar(Calendar calendar);
        
        public Task SaveCalendar(CompactCalendar compactCalendar);
    }
}