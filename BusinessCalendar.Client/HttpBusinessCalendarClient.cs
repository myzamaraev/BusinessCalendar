using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using BusinessCalendar.Client.Dto;
using Newtonsoft.Json;

namespace BusinessCalendar.Client
{
    public class HttpBusinessCalendarClient : IBusinessCalendarClient
    {
        private readonly HttpClient _httpClient;
        private const string ContentType = "application/json";
        
        public HttpBusinessCalendarClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

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