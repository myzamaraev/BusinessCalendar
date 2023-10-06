using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.WebAPI.Attributes;
using BusinessCalendar.WebAPI.Constants;
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
        public async Task<ActionResult<Calendar>> GetCalendar([FromQuery]CalendarId calendarId, CancellationToken cancellationToken = default)
        {
            var calendar = await _calendarManagementService.GetCalendarAsync(calendarId, cancellationToken);
            return Ok(calendar);
        }

        [HttpPut]
        [Route("[action]")]
        [AuthorizeRole(BcRoles.Manager)]
        public async Task<ActionResult> SaveCalendar([FromBody]SaveCalendarRequest request, CancellationToken cancellationToken = default)
        {
            await _calendarManagementService.SaveCalendarAsync(request, cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        [AuthorizeRole(BcRoles.Manager)]
        public async Task<ActionResult> SaveCompactCalendar([FromBody]SaveCompactCalendarRequest request, CancellationToken cancellationToken = default)
        {
            await _calendarManagementService.SaveCompactCalendarAsync(request, cancellationToken);
            return Ok();
        }
    }
}