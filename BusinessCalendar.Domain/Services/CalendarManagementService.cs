using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Mappers;
using BusinessCalendar.Domain.Validators;
using FluentValidation;

namespace BusinessCalendar.Domain.Services
{
    public class CalendarManagementService : ICalendarManagementService
    {
        private readonly ICalendarStorageService _calendarStorageService;
        private readonly CompactCalendarValidator _compactCalendarValidator;
        private readonly SaveCalendarRequestValidator _saveCalendarRequestValidator;
        private readonly ICalendarMapper _calendarMapper;

        public CalendarManagementService(
            ICalendarStorageService calendarStorageService, 
            CompactCalendarValidator compactCalendarValidator, 
            SaveCalendarRequestValidator saveCalendarRequestValidator,
            ICalendarMapper calendarMapper)
        {
            _calendarStorageService = calendarStorageService;
            _compactCalendarValidator = compactCalendarValidator;
            _saveCalendarRequestValidator = saveCalendarRequestValidator;
            _calendarMapper = calendarMapper;
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
            return customCalendar ?? new Calendar(type, key, year).ToCompact();
        }

        public async Task SaveCalendarAsync(SaveCalendarRequest request)
        {
            await _saveCalendarRequestValidator.ValidateAndThrowAsync(request);
            var compactCalendar = _calendarMapper.MapToCompact(request);
            await _compactCalendarValidator.ValidateAndThrowAsync(compactCalendar);
            await _calendarStorageService.Upsert(compactCalendar);
        }

        public async Task SaveCalendarAsync(SaveCompactCalendarRequest request)
        {
            var compactCalendar = _calendarMapper.MapToCompact(request);
            await _compactCalendarValidator.ValidateAndThrowAsync(compactCalendar);
            await _calendarStorageService.Upsert(compactCalendar);
        }
    }
}