using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Providers;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Validators;

namespace BusinessCalendar.Domain.Services
{
    public class CalendarManagementService : ICalendarManagementService
    {
        private readonly ICalendarStorageService _calendarStorageService;
        private readonly ICalendarIdentifierStorageService _calendarIdentifierStorageService;
        private readonly CompactCalendarValidator _compactCalendarValidator;

        public CalendarManagementService(
            ICalendarStorageService calendarStorageService, 
            ICalendarIdentifierStorageService calendarIdentifierStorageService,
            CompactCalendarValidator compactCalendarValidator)
        {
            _calendarStorageService = calendarStorageService;
            _calendarIdentifierStorageService = calendarIdentifierStorageService;
            _compactCalendarValidator = compactCalendarValidator;
        }

        public async Task AddCalendarIdentifierAsync(CalendarType type, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentOutOfRangeException(nameof(key));
            }
            
            var calendarIdentifier = new CalendarIdentifier(type, key);
            await _calendarIdentifierStorageService.InsertAsync(calendarIdentifier);
        }

        public Task<List<CalendarIdentifier>> GetCalendarIdentifiersAsync(int page, int pageSize)
        {
            if (pageSize <= 0)
            {
                return Task.FromResult(new List<CalendarIdentifier>());
            }

            var limitedPageSize = pageSize > 100 ? 100 : pageSize;
            return _calendarIdentifierStorageService.GetAllAsync(page, limitedPageSize);
        }

        public async Task<Calendar> GetCalendarAsync(CalendarType type, string key, int year)
        {
            var customCalendar = await _calendarStorageService.FindOne(type, key, year);
            //todo: provide information about persistence of the calendar
            return customCalendar != null
                ? new Calendar(customCalendar)
                : new Calendar(type, key, year);
        }

        public async Task<CompactCalendar> GetCompactCalendarAsync(CalendarType type, string key, int year)
        {
            var customCalendar = await _calendarStorageService.FindOne(type, key, year);
            return customCalendar != null
                ? customCalendar
                : new Calendar(type, key, year).ToCompact();
        }

        public async Task SaveCalendarAsync(Calendar calendar)
        {
            await SaveCalendarAsync(calendar.ToCompact());
        }

        public async Task SaveCalendarAsync(CompactCalendar compactCalendar)
        {
            var validationResult = await _compactCalendarValidator.ValidateAsync(compactCalendar);

            //todo: return validation results
            if (!validationResult.IsValid)
            {
                throw new Exception(string.Join(';', validationResult.Errors.Select(x => x.ErrorMessage)));
            }

            await _calendarStorageService.Upsert(compactCalendar);
        }
    }
}