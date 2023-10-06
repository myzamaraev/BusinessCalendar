using System;
using System.Threading.Tasks;

namespace BusinessCalendar.Client.Providers;

/// <summary>
/// WorkdayProvider provides an opportunity to check the date for workday according to specific calendar
/// </summary>
public class WorkdayProvider : IWorkdayProvider
{
    private readonly IBusinessCalendarClient _businessCalendarClient;
    private readonly WorkdayProviderOptions _options = new WorkdayProviderOptions();

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="businessCalendarClient">Provide IBusinessCalendarClient implementation to work with BusinessCalendar API</param>
    public WorkdayProvider(IBusinessCalendarClient businessCalendarClient)
        : this(businessCalendarClient, options => { })
    {
    }
        
    /// <summary>
    /// Constructor allowing additional options
    /// </summary>
    /// <param name="businessCalendarClient">Provide IBusinessCalendarClient implementation to work with BusinessCalendar API</param>
    /// <param name="options">Configure desired options, e.x. enable cache</param>
    public WorkdayProvider(IBusinessCalendarClient businessCalendarClient, Action<WorkdayProviderOptions> options)
    {
        _businessCalendarClient = businessCalendarClient;
            
        options?.Invoke(_options);
    }
        
    /// <summary>
    /// Checks the date for workday according to the calendar with specified identifier
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="date">
    /// DateTime object with appropriate timezone applied. Method uses date part as is and doesn't care about the time zone.
    /// </param>
    /// <returns>True when workday, false otherwise</returns>
    /// <throws name="ArgumentOutOfRangeException">The identifier has invalid format</throws>\
    public async Task<bool> IsWorkday(string identifier, DateTime date)
    {
        if (_options.EnableFullCalendarCache)
        {
            var calendar = await ExecuteWithOptionalCachingAsync(
                $"{nameof(_businessCalendarClient.GetCalendarAsync)}_{identifier}_{date.Year}",
                () => _businessCalendarClient.GetCalendarAsync(identifier, date.Year));

            return calendar.IsWorkday(date);
        }
            
        var getDateResponse = await ExecuteWithOptionalCachingAsync(
            $"{nameof(_businessCalendarClient.GetDateAsync)}_{identifier}_{date:yyyy-MM-dd}",
            () => _businessCalendarClient.GetDateAsync(identifier, date));

        return getDateResponse.IsWorkday;
    }


    private async Task<TItem> ExecuteWithOptionalCachingAsync<TItem>(string cacheKey, Func<Task<TItem>> func)
        where TItem: class
    {
        if (_options.IsCacheEnabled)
        {
            return await _options.CacheProvider.GetOrCreateAsync(
                $"{nameof(WorkdayProvider)}_{cacheKey}", 
                func);
        }
            
        return await func.Invoke();
    }
}