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
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarManagementService _calendarManagementService;

        public CalendarController(ICalendarManagementService calendarManagementService)
        {
            _calendarManagementService = calendarManagementService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CompactCalendar), 200)]
        public async Task<JsonResult> Get(CalendarType type, string key, int year)
        {
            var calendar = await _calendarManagementService.GetCompactCalendarAsync(type, key, year);
            return new JsonResult(calendar);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<JsonResult> GetDate(CalendarType type, string key, DateOnly date)
        {
            var calendar = await _calendarManagementService.GetCalendar(type, key, date.Year);
            return new JsonResult(calendar.Dates.Single(x => x.Date.Equals(date)));
        }
    }
}