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

    public async Task AddCalendarIdentifierAsync(AddCalendarIdentifierRequest request, CancellationToken cancellationToken = default)
    {
        await _addCalendarIdentifierRequestValidator.ValidateAndThrowAsync(request, cancellationToken);
        var calendarIdentifier = new CalendarIdentifier(request.Type, request.Key);
        await _calendarIdentifierStorageService.InsertAsync(calendarIdentifier, cancellationToken: cancellationToken);
    }

    public async Task DeleteCalendarIdentifierAsync(string id, CancellationToken cancellationToken = default)
    {
        var calendarIdentifier = await _calendarIdentifierStorageService.GetAsync(id, cancellationToken);
        if (calendarIdentifier == null)
        {
            throw new DocumentNotFoundClientException($"Calendar identifier {id} not found");
        }

        await _calendarStorageService.DeleteMany(calendarIdentifier.Type, calendarIdentifier.Key, cancellationToken);
        await _calendarIdentifierStorageService.DeleteAsync(calendarIdentifier.Id, cancellationToken);
    }

    public Task<List<CalendarIdentifier>> GetCalendarIdentifiersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page < 0) throw new ArgumentClientException(nameof(page), page.ToString());
        if (pageSize <= 0) throw new ArgumentClientException(nameof(pageSize), pageSize.ToString());

        var limitedPageSize = pageSize > 100 ? 100 : pageSize;
        return _calendarIdentifierStorageService.GetAllAsync(page, limitedPageSize, cancellationToken);
    }
}