using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Exceptions;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.WebAPI.Attributes;
using BusinessCalendar.WebAPI.Constants;
using BusinessCalendar.WebAPI.Controllers.ApiV1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCalendar.WebAPI.Controllers.BffV1
{
    public class ManagementController : BffV1Controller
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
        [AuthorizeRole(BcRoles.Manager)]
        public async Task<ActionResult> SaveCalendar([FromBody]SaveCalendarRequest request)
        {
            await _calendarManagementService.SaveCalendarAsync(request);
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        [AuthorizeRole(BcRoles.Manager)]
        public async Task<ActionResult> SaveCompactCalendar([FromBody]SaveCompactCalendarRequest request)
        {
            await _calendarManagementService.SaveCompactCalendarAsync(request);
            return Ok();
        }
    }
}