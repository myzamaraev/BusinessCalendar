using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Exceptions;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Storage;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace BusinessCalendar.Domain.Services;

public class CalendarIdentifierService : ICalendarIdentifierService
{
    private readonly ICalendarIdentifierStorageService _calendarIdentifierStorageService;
    private readonly ICalendarStorageService _calendarStorageService;
    private readonly IValidator<AddCalendarIdentifierRequest> _addCalendarIdentifierRequestValidator;
    private readonly ILogger<CalendarIdentifierService> _logger;

    public CalendarIdentifierService(
        ICalendarIdentifierStorageService calendarIdentifierStorageService, 
        ICalendarStorageService calendarStorageService,
        IValidator<AddCalendarIdentifierRequest> addCalendarIdentifierRequestValidator,
        ILogger<CalendarIdentifierService> logger)
    {
        _calendarIdentifierStorageService = calendarIdentifierStorageService;
        _calendarStorageService = calendarStorageService;
        _addCalendarIdentifierRequestValidator = addCalendarIdentifierRequestValidator;
        _logger = logger;
    }

    public async Task AddCalendarIdentifierAsync(AddCalendarIdentifierRequest request, CancellationToken cancellationToken = default)
    {
        await _addCalendarIdentifierRequestValidator.ValidateAndThrowAsync(request, cancellationToken);
        var calendarIdentifier = new CalendarIdentifier(request.Type, request.Key);
        await _calendarIdentifierStorageService.InsertAsync(calendarIdentifier, cancellationToken: cancellationToken);
        _logger.LogCreated(calendarIdentifier);
    }

    public async Task DeleteCalendarIdentifierAsync(string id, CancellationToken cancellationToken = default)
    {
        var calendarIdentifier = await _calendarIdentifierStorageService.GetAsync(id, cancellationToken);
        if (calendarIdentifier == null)
        {
            _logger.LogCantDeleteNotExisting(id);
            throw new DocumentNotFoundClientException(nameof(CalendarIdentifier), id);
        }

        var deletedCount = await _calendarStorageService.DeleteManyAsync(calendarIdentifier.Type, calendarIdentifier.Key, cancellationToken);
        _logger.LogCalendarsDeleted(calendarIdentifier, deletedCount);
        
        await _calendarIdentifierStorageService.DeleteAsync(calendarIdentifier.Id, cancellationToken);
        _logger.LogDeleted(calendarIdentifier);
    }

    public Task<List<CalendarIdentifier>> GetCalendarIdentifiersAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        if (page < 0) throw new ArgumentClientException(nameof(page), page.ToString());
        if (pageSize <= 0) throw new ArgumentClientException(nameof(pageSize), pageSize.ToString());
        
        pageSize = LimitPageSize(pageSize, 100);
        
        return _calendarIdentifierStorageService.GetAllAsync(page, pageSize, cancellationToken);
    }

    private int LimitPageSize(int requestedPageSize, int pageLimit)
    {
        var limitedPageSize = requestedPageSize > pageLimit ? pageLimit : requestedPageSize;
        if (requestedPageSize != limitedPageSize)
        {
            _logger.LogPageSizeLimited(limitedPageSize);
        }

        return limitedPageSize;
    }
    
    public Task<CalendarIdentifier?> GetAsync(string id, CancellationToken cancellationToken = default) => _calendarIdentifierStorageService.GetAsync(id, cancellationToken);
}