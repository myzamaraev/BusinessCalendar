using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Services;

public interface ICalendarIdentifierService
{
    public Task AddCalendarIdentifierAsync(CalendarType type, string key);

    public Task DeleteCalendarIdentifierAsync(string id);

    public Task<List<CalendarIdentifier>> GetCalendarIdentifiersAsync(int page, int pageSize);
}