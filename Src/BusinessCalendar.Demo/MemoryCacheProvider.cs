using BusinessCalendar.Client.Providers.Dependencies;
using Microsoft.Extensions.Caching.Memory;

namespace BusinessCalendar.Demo;

public class MemoryCacheProvider : ICacheProvider
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheProvider(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    
    public Task<TItem?> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> func)
    {
        return _memoryCache.GetOrCreateAsync(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            return func.Invoke();
        });
    }
}