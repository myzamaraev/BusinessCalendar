using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using BusinessCalendar.Client.Dto;
using Newtonsoft.Json;

namespace BusinessCalendar.Client
{
    /// <summary>
    /// Client realization accessing BusinessCalendar API through HTTP protocol
    /// </summary>
    public class HttpBusinessCalendarClient : IBusinessCalendarClient
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
            //todo: replace type and key with identifier to move logic to backend?
            //or might it be reasonable to parse identifier in the client?
            var (type, key) = ParseIdentifier(identifier);
            var path = $"/api/v1/Calendar/{type}/{key}/{year}";
            var response = await _httpClient.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var calendarModel = Deserialize<CalendarModel>(responseStream);
                return calendarModel;
            }

            var content = await response.Content.ReadAsStringAsync();
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
            var (type, key) = ParseIdentifier(identifier);
            
             const string path = "/api/v1/Calendar/GetDate";
             var uriParameters = string.Join("&" , 
                 $"type={HttpUtility.UrlEncode(type)}",
                 $"key={HttpUtility.UrlEncode(key)}",
                 $"{nameof(date)}={date:yyyy-MM-dd}");
            
             var pathWithParameters = string.Join("?", path, uriParameters);
             
            var response = await _httpClient.GetAsync(pathWithParameters);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                
                var calendarDate = Deserialize<CalendarDateModel>(responseStream);
                return calendarDate;
            }

            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(content);
        }

        private static (string type, string key) ParseIdentifier(string identifier)
        {
            IdentifierRegex.EnsureMatch(identifier); 

            var firstUnderscoreIndex = identifier.IndexOf('_');
            var type = identifier.Substring(0, firstUnderscoreIndex);
            var key = identifier.Substring(firstUnderscoreIndex + 1, identifier.Length-firstUnderscoreIndex-1);

            return (type, key);
        }
        
        private static T Deserialize<T>(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            using (var reader = new JsonTextReader(sr))
            {
                return new JsonSerializer().Deserialize<T>(reader);
            }
        }
    }
}