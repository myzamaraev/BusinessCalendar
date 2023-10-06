using BusinessCalendar.Client.Providers.Dependencies;

namespace BusinessCalendar.Client.Providers;

/// <summary>
/// An options class for WorkdayProvider
/// </summary>
public class WorkdayProviderOptions
{
    internal ICacheProvider CacheProvider { get; private set; }

    internal bool EnableFullCalendarCache { get; set; }

    /// <summary>
    /// Use this prop to check if cache enabled or not
    /// </summary>
    public bool IsCacheEnabled => CacheProvider != null;

    /// <summary>
    /// Enables API response cache
    /// </summary>
    /// <param name="cacheProvider"></param>
    public void UseCache(ICacheProvider cacheProvider)
    {
        CacheProvider = cacheProvider;
    }

    /// <summary>
    /// Enables full year calendar cache to prevent separate request per each date inside the year
    /// Forces usage of client's GetCalendar method instead of GetCalendarDate
    /// </summary>
    /// <param name="cacheProvider"></param>
    public void UseFullCalendarCache(ICacheProvider cacheProvider)
    {
        CacheProvider = cacheProvider;
        EnableFullCalendarCache = true;
    }
}