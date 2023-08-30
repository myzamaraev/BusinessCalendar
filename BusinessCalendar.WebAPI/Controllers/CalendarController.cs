using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Contracts.ApiContracts;
using Microsoft.AspNetCore.Mvc;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.WebAPI.Controllers
{
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
            var response = new GetCalendarResponse()
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
        public async Task<ActionResult<CalendarDate>> GetDate(CalendarType type, string key, DateOnly date)
        {
            var calendar = await _calendarManagementService.GetCalendarAsync(
                new CalendarId()
                {
                    Type = type,
                    Key = key,
                    Year = date.Year
                });
            
            var calendarDate = calendar.Dates.Single(x => x.Date.Equals(date));

            var response = new GetCalendarDateResponse()
            {
                Type = calendar.Id.Type.ToString(),
                Key = calendar.Id.Key,
                Year = calendar.Id.Year,
                Date = calendarDate.Date.ToDateTime(new TimeOnly(), DateTimeKind.Utc),
                IsWorkday = calendarDate.IsWorkday
            };
            
            return Ok(response);
        }
    }
}