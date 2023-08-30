using System.Text.Json;
using BusinessCalendar.Client;
using BusinessCalendar.Client.Providers;
using BusinessCalendar.Client.Providers.Dependencies;
using BusinessCalendar.Demo;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;

services.AddHttpClient<IBusinessCalendarClient, HttpBusinessCalendarClient>((httpClient) =>
{
    httpClient.BaseAddress = new Uri("http://localhost:4080/");
});

//Without caching
services.AddSingleton<IWorkdayProvider, WorkdayProvider>();

//With cache enabled
services.AddMemoryCache(); //You can use any other cache you want inside ICacheProvider implementation
services.AddSingleton<ICacheProvider, MemoryCacheProvider>(); //we use custom ICacheProvider to get rid of specific dependencies
services.AddSingleton<IWorkdayProvider, WorkdayProvider>((sp) =>
 {
         var client =  sp.GetRequiredService<IBusinessCalendarClient>();
         var cacheProvider = sp.GetRequiredService<ICacheProvider>();
         return new WorkdayProvider(client, (options) =>
         {
             options.UseCache(cacheProvider); //if we just want to cache Calendar/GetDate responses
             options.UseFullCalendarCache(cacheProvider); //if we want to request full year calendar once, and use it many times
         });
     });

services.AddHostedService<ExampleService>();

using var host = builder.Build();
await host.RunAsync();
    