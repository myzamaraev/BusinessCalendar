using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.WebAPI.Attributes;
using BusinessCalendar.WebAPI.Constants;
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

        [HttpPut]
        [AuthorizeRole(BcRoles.Manager)]
        public async Task<ActionResult> CalendarIdentifier([FromBody]AddCalendarIdentifierRequest request, CancellationToken cancellationToken) 
        {
            await _calendarIdentifierService.AddCalendarIdentifierAsync(request, cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [AuthorizeRole(BcRoles.Manager)]
        public async Task<ActionResult> CalendarIdentifier(string id, CancellationToken cancellationToken)
        {
            await _calendarIdentifierService.DeleteCalendarIdentifierAsync(id, cancellationToken);
            return Ok();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<CalendarIdentifier>>> List(int page = 0, int pageSize = 20, CancellationToken cancellationToken = default) 
        {
            var calendarIdentifiers = await _calendarIdentifierService.GetCalendarIdentifiersAsync(page, pageSize, cancellationToken);
            return Ok(calendarIdentifiers);
        }
    }
}