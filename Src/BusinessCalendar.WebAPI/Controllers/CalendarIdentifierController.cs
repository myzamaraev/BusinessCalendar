using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Dto;
using System.Linq;
using BusinessCalendar.Domain.Dto.Requests;

namespace BusinessCalendar.WebAPI.Controllers
{
    public class CalendarIdentifierController : ApiV1Controller
    {
        private readonly ICalendarIdentifierService _calendarIdentifierService;

        public CalendarIdentifierController(ICalendarIdentifierService calendarIdentifierService)
        {
            _calendarIdentifierService = calendarIdentifierService;
        }

        [HttpPost]
        public async Task<ActionResult> CalendarIdentifier([FromBody]AddCalendarIdentifierRequest request) 
        {
            await _calendarIdentifierService.AddCalendarIdentifierAsync(request);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
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