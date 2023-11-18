using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Mappers;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace BusinessCalendar.Domain.Services;

public class CalendarManagementService : ICalendarManagementService
{
    private readonly ICalendarStorageService _calendarStorageService;
    private readonly IValidator<CompactCalendar> _compactCalendarValidator;
    private readonly IValidator<SaveCalendarRequest> _saveCalendarRequestValidator;
    private readonly IValidator<CalendarId> _calendarIdValidator;
    private readonly ICalendarMapper _calendarMapper;
    private readonly ILogger<CalendarManagementService> _logger;

    public CalendarManagementService(
        ICalendarStorageService calendarStorageService, 
        IValidator<CompactCalendar> compactCalendarValidator, 
        IValidator<SaveCalendarRequest> saveCalendarRequestValidator,
        IValidator<CalendarId> calendarIdValidator,
        ICalendarMapper calendarMapper,
        ILogger<CalendarManagementService> logger)
    {
        _calendarStorageService = calendarStorageService;
        _compactCalendarValidator = compactCalendarValidator;
        _saveCalendarRequestValidator = saveCalendarRequestValidator;
        _calendarIdValidator = calendarIdValidator;
        _calendarMapper = calendarMapper;
        _logger = logger;
    }
        
    public async Task<Calendar> GetCalendarAsync(CalendarId calendarId, CancellationToken cancellationToken = default)
    {
        await _calendarIdValidator.ValidateAndThrowAsync(calendarId, cancellationToken);
        var compactCalendar = await _calendarStorageService.FindOneAsync(calendarId, cancellationToken);
        return compactCalendar != null 
            ? _calendarMapper.Map(compactCalendar) 
            : ProvideDefaultCalendar(calendarId);
    }

    public async Task<CompactCalendar> GetCompactCalendarAsync(CalendarId calendarId, CancellationToken cancellationToken = default)
    {
        await _calendarIdValidator.ValidateAndThrowAsync(calendarId, cancellationToken);
        var compactCalendar = await _calendarStorageService.FindOneAsync(calendarId, cancellationToken);

        return compactCalendar ?? ProvideDefaultCalendar(calendarId).ToCompact();
    }

    public async Task SaveCalendarAsync(SaveCalendarRequest request, CancellationToken cancellationToken = default)
    {
        await _saveCalendarRequestValidator.ValidateAndThrowAsync(request, cancellationToken);
        var compactCalendar = _calendarMapper.MapToCompact(request);
        await _compactCalendarValidator.ValidateAndThrowAsync(compactCalendar, cancellationToken);
        await _calendarStorageService.UpsertAsync(compactCalendar, cancellationToken);
        _logger.LogCalendarUpdated(compactCalendar);
    }

    public async Task SaveCompactCalendarAsync(SaveCompactCalendarRequest request, CancellationToken cancellationToken = default)
    { 
        var compactCalendar = _calendarMapper.MapToCompact(request);
        await _compactCalendarValidator.ValidateAndThrowAsync(compactCalendar, cancellationToken);
        await _calendarStorageService.UpsertAsync(compactCalendar, cancellationToken);
        _logger.LogCalendarUpdated(compactCalendar);
    }

    private Calendar ProvideDefaultCalendar(CalendarId calendarId)
    {
        _logger.LogDefaultCalendarUsageWarning(calendarId);
        return new Calendar(calendarId);
    }
}