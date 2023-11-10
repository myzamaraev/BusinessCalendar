using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.Domain.Storage;

public interface ICalendarIdentifierStorageService
{
    public Task InsertAsync(CalendarIdentifier calendarIdentifier, CancellationToken cancellationToken = default);

    public Task<List<CalendarIdentifier>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    public Task<CalendarIdentifier?> GetAsync(string id, CancellationToken cancellationToken = default);
        
    public Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}