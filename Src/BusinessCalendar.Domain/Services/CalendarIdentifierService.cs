using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Exceptions;
using BusinessCalendar.Domain.Storage;
using FluentValidation;

namespace BusinessCalendar.Domain.Services;

public class CalendarIdentifierService : ICalendarIdentifierService
{
    private readonly ICalendarIdentifierStorageService _calendarIdentifierStorageService;
    private readonly ICalendarStorageService _calendarStorageService;
    private readonly IValidator<AddCalendarIdentifierRequest> _addCalendarIdentifierRequestValidator;

    public CalendarIdentifierService(
        ICalendarIdentifierStorageService calendarIdentifierStorageService, 
        ICalendarStorageService calendarStorageService,
        IValidator<AddCalendarIdentifierRequest> addCalendarIdentifierRequestValidator)
    {
        _calendarIdentifierStorageService = calendarIdentifierStorageService;
        _calendarStorageService = calendarStorageService;
        _addCalendarIdentifierRequestValidator = addCalendarIdentifierRequestValidator;
    }

    public async Task AddCalendarIdentifierAsync(AddCalendarIdentifierRequest request)
    {
        await _addCalendarIdentifierRequestValidator.ValidateAndThrowAsync(request);
        var calendarIdentifier = new CalendarIdentifier(request.Type, request.Key);
        await _calendarIdentifierStorageService.InsertAsync(calendarIdentifier);
    }

    public async Task DeleteCalendarIdentifierAsync(string id)
    {
        var calendarIdentifier = await _calendarIdentifierStorageService.GetAsync(id);
        if (calendarIdentifier == null)
        {
            throw new DocumentNotFoundClientException($"Calendar identifier {id} not found");
        }

        await _calendarStorageService.DeleteMany(calendarIdentifier.Type, calendarIdentifier.Key);
        await _calendarIdentifierStorageService.DeleteAsync(calendarIdentifier.Id);
    }

    public Task<List<CalendarIdentifier>> GetCalendarIdentifiersAsync(int page, int pageSize)
    {
        if (page < 0) throw new ArgumentClientException(nameof(page), page.ToString());
        if (pageSize <= 0) throw new ArgumentClientException(nameof(pageSize), pageSize.ToString());

        var limitedPageSize = pageSize > 100 ? 100 : pageSize;
        return _calendarIdentifierStorageService.GetAllAsync(page, limitedPageSize);
    }
}