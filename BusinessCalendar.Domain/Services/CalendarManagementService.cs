using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Providers;

namespace BusinessCalendar.Domain.Services
{
    public class CalendarManagementService : ICalendarManagementService
    {
        private readonly ICalendarStorageService calendarStorageService;

        public CalendarManagementService(ICalendarStorageService calendarStorageService)
        {
            this.calendarStorageService = calendarStorageService;
        }

        public async Task<Calendar> GetCalendar(CalendarType type, string key, int year) 
        {
            var customCalendar = await calendarStorageService.FindOne(type, key, year);
            return customCalendar != null 
                ? new Calendar(customCalendar)
                : new Calendar(type, key, year);
        }

        public async Task<CompactCalendar> GetCompactCalendarAsync(CalendarType type, string key, int year) 
        {
            var customCalendar = await calendarStorageService.FindOne(type, key, year);
            return customCalendar != null 
                ? customCalendar
                : new CompactCalendar(new Calendar(type, key, year));
            //todo: replace constructor to ToCompact extention?
        }

        public async Task SaveCalendar(Calendar calendar)
        {
            var customCalendar = new CompactCalendar(calendar);
            await calendarStorageService.Upsert(customCalendar);
        }
    }
}