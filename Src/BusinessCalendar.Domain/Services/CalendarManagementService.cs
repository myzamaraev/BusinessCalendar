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
        private readonly IValidator<CompactCalendar> _compactCalendarValidator;
        private readonly IValidator<SaveCalendarRequest> _saveCalendarRequestValidator;
        private readonly ICalendarMapper _calendarMapper;

        public CalendarManagementService(
            ICalendarStorageService calendarStorageService, 
            IValidator<CompactCalendar> compactCalendarValidator, 
            IValidator<SaveCalendarRequest> saveCalendarRequestValidator,
            ICalendarMapper calendarMapper)
        {
            _calendarStorageService = calendarStorageService;
            _compactCalendarValidator = compactCalendarValidator;
            _saveCalendarRequestValidator = saveCalendarRequestValidator;
            _calendarMapper = calendarMapper;
        }
        
        public async Task<Calendar> GetCalendarAsync(CalendarId calendarId)
        {
            var compactCalendar = await _calendarStorageService.FindOne(calendarId);
            //todo: provide information about persistence of the calendar
            return compactCalendar != null
                ? _calendarMapper.Map(compactCalendar)
                : new Calendar(calendarId);
        }

        public async Task<CompactCalendar> GetCompactCalendarAsync(CalendarId calendarId)
        {
            var customCalendar = await _calendarStorageService.FindOne(calendarId);
            return customCalendar ?? new Calendar(calendarId).ToCompact();
        }

        public async Task SaveCalendarAsync(SaveCalendarRequest request)
        {
            await _saveCalendarRequestValidator.ValidateAndThrowAsync(request);
            var compactCalendar = _calendarMapper.MapToCompact(request);
            await _compactCalendarValidator.ValidateAndThrowAsync(compactCalendar);
            await _calendarStorageService.Upsert(compactCalendar);
        }

        public async Task SaveCompactCalendarAsync(SaveCompactCalendarRequest request)
        { 
            var compactCalendar = _calendarMapper.MapToCompact(request);
            await _compactCalendarValidator.ValidateAndThrowAsync(compactCalendar);
            await _calendarStorageService.Upsert(compactCalendar);
        }
    }
}