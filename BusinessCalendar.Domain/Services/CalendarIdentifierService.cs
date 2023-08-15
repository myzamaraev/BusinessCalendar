using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Storage;

namespace BusinessCalendar.Domain.Services;

public class CalendarIdentifierService
{
    private readonly ICalendarIdentifierStorageService _calendarIdentifierStorageService;
    private readonly ICalendarStorageService _calendarStorageService;

    public CalendarIdentifierService(
        ICalendarIdentifierStorageService calendarIdentifierStorageService, 
        ICalendarStorageService calendarStorageService)
    {
        _calendarIdentifierStorageService = calendarIdentifierStorageService;
        _calendarStorageService = calendarStorageService;
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

    public async Task DeleteCalendarIdentifierAsync(string id)
    {
        var calendarIdentifier = await _calendarIdentifierStorageService.GetAsync(id);
        if (calendarIdentifier == null)
        {
            throw new Exception($"Calendar identifier {id} not found");
        }

        await _calendarStorageService.DeleteMany(calendarIdentifier.Type, calendarIdentifier.Key);
        await _calendarIdentifierStorageService.DeleteAsync(calendarIdentifier.Id);
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
}