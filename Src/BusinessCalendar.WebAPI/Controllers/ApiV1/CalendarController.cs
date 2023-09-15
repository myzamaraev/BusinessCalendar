using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCalendar.WebAPI.Controllers.ApiV1
{
    [AllowAnonymous]
    public class CalendarController : ApiV1Controller
    {
        private readonly ICalendarManagementService _calendarManagementService;

        public CalendarController(ICalendarManagementService calendarManagementService)
        {
            _calendarManagementService = calendarManagementService;
        }

        [HttpGet]
        [Route("{Type}/{Key}/{Year}")]
        [ProducesResponseType(typeof(GetCalendarResponse), 200)]
        public async Task<ActionResult<GetCalendarResponse>> Get([FromRoute]CalendarId calendarId)
        {
            var calendar = await _calendarManagementService.GetCompactCalendarAsync(calendarId);
            
            var response = new GetCalendarResponse
            {
                Type = calendar.Id.Type.ToString(),
                Key = calendar.Id.Key,
                Year = calendar.Id.Year,
                Holidays = calendar.Holidays
                    .Select(x => x.ToDateTime(new TimeOnly(), DateTimeKind.Utc))
                    .ToList(),
                ExtraWorkDays = calendar.ExtraWorkDays
                    .Select(x => x.ToDateTime(new TimeOnly(), DateTimeKind.Utc))
                    .ToList(),
            };
            return Ok(response);
        }
        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(GetCalendarDateResponse), 200)]
        public async Task<ActionResult<GetCalendarDateResponse>> GetDate(CalendarType type, string key, DateOnly date)
        {
            var calendar = await _calendarManagementService.GetCalendarAsync(
                new CalendarId()
                {
                    Type = type,
                    Key = key,
                    Year = date.Year
                });
            
            var calendarDate = calendar.Dates.Single(x => x.Date.Equals(date));

            var response = new GetCalendarDateResponse
            {
                Type = calendar.Id.Type.ToString(),
                Key = calendar.Id.Key,
                Date = calendarDate.Date.ToDateTime(new TimeOnly(), DateTimeKind.Utc),
                IsWorkday = calendarDate.IsWorkday
            };
            
            return Ok(response);
        }
    }
}