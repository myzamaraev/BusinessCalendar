using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Providers;
using BusinessCalendar.Domain.Extensions;

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
                : new Calendar(type, key, year).ToCompact();
        }

        public async Task SaveCalendar(Calendar calendar)
        {
            await calendarStorageService.Upsert(calendar.ToCompact());
        }

        public async Task SaveCalendar(CompactCalendar compactCalendar)
        {
            //todo: make validation through FluentValidation
            if (compactCalendar.Holidays.Any(holiday => holiday.Year != compactCalendar.Id.Year))
            {
                throw new Exception("Every date in Holidays array must be part of the year");
            };

            if (compactCalendar.ExtraWorkDays.Any(extraWorkDay => extraWorkDay.Year != compactCalendar.Id.Year))
            {
                throw new Exception("Every date in ExtraWorkDays array must be part of the year");
            };

            if (compactCalendar.Holidays.Distinct().Count() != compactCalendar.Holidays.Count())
            {
                throw new Exception("Holidays array has duplicate dates");
            }

            if (compactCalendar.ExtraWorkDays.Distinct().Count() != compactCalendar.ExtraWorkDays.Count())
            {
                throw new Exception("ExtraWorkDays array has duplicate dates");
            }


            await calendarStorageService.Upsert(compactCalendar);
        }
    }
}