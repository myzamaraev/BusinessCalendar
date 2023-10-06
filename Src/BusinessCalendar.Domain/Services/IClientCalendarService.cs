using BusinessCalendar.Domain.Dto.Responses;

namespace BusinessCalendar.Domain.Services;

public interface IClientCalendarService
{
    /// <summary>
    /// Searches a calendar by identifier and yer
    /// </summary>
    /// <param name="identifier">Identifier of the calendar</param>
    /// <param name="year">Year to find</param>
    /// <param name="cancellationToken"></param>
    /// <returns>GetCalendarResponse object</returns>
    /// <exception cref="WrongIdentifierFormatClientException"></exception>
    Task<GetCalendarResponse> GetCalendarAsync(string identifier, int year, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Searches a calendar date for a particular identifier
    /// </summary>
    /// <param name="identifier">Identifier of the calendar</param>
    /// <param name="date">Date to find</param>
    /// <param name="cancellationToken"></param>
    /// <returns>GetCalendarDateResponse object</returns>
    /// <exception cref="WrongIdentifierFormatClientException"></exception>
    Task<GetCalendarDateResponse> GetCalendarDateAsync(string identifier, DateOnly date, CancellationToken cancellationToken = default);
}