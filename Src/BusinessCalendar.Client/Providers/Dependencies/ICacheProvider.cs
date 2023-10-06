using System;
using System.Threading.Tasks;

namespace BusinessCalendar.Client.Providers.Dependencies;

/// <summary>
/// An interface for caching mechanism abstraction
/// Should be implemented to enable caching
/// </summary>
public interface ICacheProvider
{
    /// <summary>
    /// Gets the entry of TItem from cache, or creates new one with the help of createItemFunc
    /// Feel free to use any kind of cache you want
    /// </summary>
    /// <param name="key">Cache key</param>
    /// <param name="createItemFunc">delegate to create new entry</param>
    /// <typeparam name="TItem"></typeparam>
    /// <returns>Existing or newly created entry from cache</returns>
    Task<TItem> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> createItemFunc);
}