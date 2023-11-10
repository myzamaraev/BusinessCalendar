using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.Domain.Storage;

public interface ICalendarStorageService
{
    public Task UpsertAsync(CompactCalendar compactCalendar, CancellationToken cancellationToken = default);
    public Task<CompactCalendar?> FindOneAsync(CalendarId id, CancellationToken cancellationToken = default);
    public Task DeleteManyAsync(CalendarType type, string key, CancellationToken cancellationToken = default);
}