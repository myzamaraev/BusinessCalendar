using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;

namespace BusinessCalendar.Domain.Services;

public interface ICalendarManagementService
{
    public Task<Calendar> GetCalendarAsync(CalendarId calendarId, CancellationToken cancellationToken = default);

    public Task<CompactCalendar> GetCompactCalendarAsync(CalendarId calendarId, CancellationToken cancellationToken = default);

    public Task SaveCalendarAsync(SaveCalendarRequest request, CancellationToken cancellationToken = default);
        
    public Task SaveCompactCalendarAsync(SaveCompactCalendarRequest request, CancellationToken cancellationToken = default);
        
}