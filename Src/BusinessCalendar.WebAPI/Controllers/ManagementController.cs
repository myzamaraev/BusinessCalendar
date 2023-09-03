using Microsoft.AspNetCore.Mvc;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Exceptions;

namespace BusinessCalendar.WebAPI.Controllers
{
    public class ManagementController : ApiV1Controller
    {
        private readonly ICalendarManagementService _calendarManagementService;

        public ManagementController(ICalendarManagementService calendarManagementService)
        {
            _calendarManagementService = calendarManagementService;
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Calendar>> GetCalendar([FromQuery]CalendarId calendarId)
        {
            var calendar = await _calendarManagementService.GetCalendarAsync(calendarId);
            return Ok(calendar);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> SaveCalendar([FromBody]SaveCalendarRequest request)
        {
            await _calendarManagementService.SaveCalendarAsync(request);
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        public async Task<ActionResult> SaveCompactCalendar([FromBody]SaveCompactCalendarRequest request)
        {
            await _calendarManagementService.SaveCompactCalendarAsync(request);
            return Ok();
        }
    }
}