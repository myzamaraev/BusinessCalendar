using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;

namespace BusinessCalendar.Domain.Services;

public interface ICalendarIdentifierService
{
    public Task AddCalendarIdentifierAsync(
        AddCalendarIdentifierRequest request, CancellationToken cancellationToken = default);

    public Task DeleteCalendarIdentifierAsync(string id, CancellationToken cancellationToken = default);

    public Task<List<CalendarIdentifier>> GetCalendarIdentifiersAsync(
        int page, int pageSize, CancellationToken cancellationToken = default);

    public Task<CalendarIdentifier?> GetAsync(string id, CancellationToken cancellationToken = default);
}