using System;
using System.Threading.Tasks;

namespace BusinessCalendar.Client.Providers.Dependencies
{
    public interface ICacheProvider
    {
        Task<TItem> GetOrCreateAsync<TItem>(string key, Func<Task<TItem>> func);
    }
}