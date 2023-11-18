using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Responses;
using BusinessCalendar.Domain.Exceptions;

namespace BusinessCalendar.Domain.Services;

public class ClientCalendarService : IClientCalendarService
{
    private readonly ICalendarManagementService _calendarManagementService;
    private readonly ICalendarIdentifierService _calendarIdentifierService;

    public ClientCalendarService(ICalendarManagementService calendarManagementService, ICalendarIdentifierService calendarIdentifierService)
    {
        _calendarManagementService = calendarManagementService;
        _calendarIdentifierService = calendarIdentifierService;
    }

    public async Task<GetCalendarResponse> GetCalendarAsync(string identifier, int year, CancellationToken cancellationToken = default)
    {
        var calendarIdentifier = await _calendarIdentifierService.GetAsync(identifier, cancellationToken);
        if (calendarIdentifier == null)
        {
            throw new DocumentNotFoundClientException(nameof(CalendarIdentifier), identifier);
        }
        
        var compactCalendar = await _calendarManagementService.GetCompactCalendarAsync(new CalendarId(calendarIdentifier.Type, calendarIdentifier.Key, year), cancellationToken);
            
        var response = new GetCalendarResponse
        {
            Type = compactCalendar.Id.Type.ToString(),
            Key = compactCalendar.Id.Key,
            Year = compactCalendar.Id.Year,
            Holidays = compactCalendar.Holidays
                .Select(x => x.ToDateTime(new TimeOnly(), DateTimeKind.Utc))
                .ToList(),
            ExtraWorkDays = compactCalendar.ExtraWorkDays
                .Select(x => x.ToDateTime(new TimeOnly(), DateTimeKind.Utc))
                .ToList(),
        };

        return response;
    }

    public async Task<GetCalendarDateResponse> GetCalendarDateAsync(string identifier, DateOnly date, CancellationToken cancellationToken = default)
    {
        var calendarIdentifier = await _calendarIdentifierService.GetAsync(identifier, cancellationToken);
        if (calendarIdentifier == null)
        {
            throw new DocumentNotFoundClientException(nameof(CalendarIdentifier), identifier);
        }
        
        var calendar = await _calendarManagementService.GetCalendarAsync(new CalendarId(calendarIdentifier.Type, calendarIdentifier.Key, date.Year), cancellationToken);
        var calendarDate = calendar.Dates.Single(x => x.Date.Equals(date));

        var response = new GetCalendarDateResponse
        {
            Type = calendar.Id.Type.ToString(),
            Key = calendar.Id.Key,
            Date = calendarDate.Date.ToDateTime(new TimeOnly(), DateTimeKind.Utc),
            IsWorkday = calendarDate.IsWorkday
        };

        return response;
    }
}