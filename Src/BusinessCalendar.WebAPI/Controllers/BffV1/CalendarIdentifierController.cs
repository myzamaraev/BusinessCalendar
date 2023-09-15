using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.WebAPI.Attributes;
using BusinessCalendar.WebAPI.Constants;
using BusinessCalendar.WebAPI.Controllers.ApiV1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCalendar.WebAPI.Controllers.BffV1
{
    public class CalendarIdentifierController : BffV1Controller
    {
        private readonly ICalendarIdentifierService _calendarIdentifierService;

        public CalendarIdentifierController(ICalendarIdentifierService calendarIdentifierService)
        {
            _calendarIdentifierService = calendarIdentifierService;
        }

        [HttpPost]
        [AuthorizeRole(BcRoles.Manager)]
        public async Task<ActionResult> CalendarIdentifier([FromBody]AddCalendarIdentifierRequest request) 
        {
            await _calendarIdentifierService.AddCalendarIdentifierAsync(request);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [AuthorizeRole(BcRoles.Manager)]
        public async Task<ActionResult> CalendarIdentifier(string id)
        {
            await _calendarIdentifierService.DeleteCalendarIdentifierAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<CalendarIdentifier>>> List(int page = 0, int pageSize = 20) 
        {
            var calendarIdentifiers = await _calendarIdentifierService.GetCalendarIdentifiersAsync(page, pageSize);
            return Ok(calendarIdentifiers);
        }
    }
}