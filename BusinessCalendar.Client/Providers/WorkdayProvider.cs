using System;
using System.Threading.Tasks;
namespace BusinessCalendar.Client.Providers
{
    public class WorkdayProvider : IWorkdayProvider
    {
        private readonly IBusinessCalendarClient _businessCalendarClient;
        private readonly WorkdayProviderOptions _options = new WorkdayProviderOptions();

        public WorkdayProvider(IBusinessCalendarClient businessCalendarClient)
            : this(businessCalendarClient, options => { })
        {
        }
        
        public WorkdayProvider(IBusinessCalendarClient businessCalendarClient, Action<WorkdayProviderOptions> options)
        {
            _businessCalendarClient = businessCalendarClient;
            
            options?.Invoke(_options);
        }
        
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
}