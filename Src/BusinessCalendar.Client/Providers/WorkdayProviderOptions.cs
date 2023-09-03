using BusinessCalendar.Client.Providers.Dependencies;

namespace BusinessCalendar.Client.Providers
{
    public class WorkdayProviderOptions
    {
        internal ICacheProvider CacheProvider { get; private set; }

        internal bool EnableFullCalendarCache { get; set; }

        public bool IsCacheEnabled => CacheProvider != null;

        /// <summary>
        /// Enables client response cache
        /// </summary>
        /// <param name="cacheProvider"></param>
        public void UseCache(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
        }

        /// <summary>
        /// Enables full year calendar cache to prevent separate request per each date inside the year
        /// </summary>
        /// <param name="cacheProvider"></param>
        public void UseFullCalendarCache(ICacheProvider cacheProvider)
        {
            CacheProvider = cacheProvider;
            EnableFullCalendarCache = true;
        }
    }
}