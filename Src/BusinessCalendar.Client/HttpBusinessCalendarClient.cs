using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using BusinessCalendar.Client.Dto;
using Newtonsoft.Json;

namespace BusinessCalendar.Client;

/// <summary>
/// Client realization accessing BusinessCalendar API through HTTP protocol
/// </summary>
public sealed class HttpBusinessCalendarClient : IBusinessCalendarClient
{
    private readonly HttpClient _httpClient;
    private const string ContentType = "application/json";
        
    /// <summary>
    /// Provide HttpClient to use this client
    /// For .net Core 2.1 and higher it is recommended to use HttpClientFactory and Typed Clients (Service Agent pattern)
    /// </summary>
    /// <param name="httpClient"></param>
    public HttpBusinessCalendarClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Returns the calendar for provided identifier and year
    /// </summary>
    /// <param name="identifier">Calendar identifier, find it on the calendar settings page</param>
    /// <param name="year">An integer value representing year</param>
    /// <returns name="CalendarDateModel"></returns>
    public async Task<CalendarModel> GetCalendarAsync(string identifier, int year)
    {
        var path = $"/api/v1/Calendar/{HttpUtility.UrlEncode(identifier)}/{year}";
        var response = await _httpClient.GetAsync(path).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var calendarModel = Deserialize<CalendarModel>(responseStream);
            return calendarModel;
        }

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        throw new HttpRequestException(content);
    }

    /// <summary>
    /// Returns the information about particular calendar date
    /// </summary>
    /// <param name="identifier">Calendar identifier, find it on the calendar settings page</param>
    /// <param name="date"></param>
    /// <returns name="CalendarDateModel"></returns>
    public async Task<CalendarDateModel> GetDateAsync(string identifier, DateTime date)
    {
        const string path = "/api/v1/Calendar/GetDate";
        var uriParameters = string.Join("&" , 
            $"identifier={HttpUtility.UrlEncode(identifier)}",
            $"{nameof(date)}={date:yyyy-MM-dd}");
            
        var pathWithParameters = string.Join("?", path, uriParameters);
             
        var response = await _httpClient.GetAsync(pathWithParameters).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                
            var calendarDate = Deserialize<CalendarDateModel>(responseStream);
            return calendarDate;
        }

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        throw new HttpRequestException(content);
    }

    private static T Deserialize<T>(Stream stream)
    {
        using var sr = new StreamReader(stream);
        using var reader = new JsonTextReader(sr);
        return new JsonSerializer().Deserialize<T>(reader);
    }
}