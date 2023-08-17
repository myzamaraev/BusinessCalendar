using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<CompactCalendar>> Get([FromRoute]CalendarId calendarId)
        {
            var calendar = await _calendarManagementService.GetCompactCalendarAsync(calendarId);
            return Ok(calendar);
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
            
            return Ok(calendar.Dates.Single(x => x.Date.Equals(date)));
        }
    }
}