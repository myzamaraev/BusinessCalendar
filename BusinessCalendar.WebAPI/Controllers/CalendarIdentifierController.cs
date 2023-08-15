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
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalendarIdentifierController : ControllerBase
    {
        private readonly ICalendarIdentifierService _calendarIdentifierService;

        public CalendarIdentifierController(ICalendarIdentifierService calendarIdentifierService)
        {
            _calendarIdentifierService = calendarIdentifierService;
        }

        [HttpPost]
        public async Task CalendarIdentifier([FromBody]AddCalendarRequest request) 
        {
            await _calendarIdentifierService.AddCalendarIdentifierAsync(request.Type, request.Key);
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [Route("{id}")]
        public async Task CalendarIdentifier(string id)
        {
            await _calendarIdentifierService.DeleteCalendarIdentifierAsync(id);
            BadRequest(ModelState);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<CalendarIdentifier>), 200)]
        public async Task<JsonResult> List(int page = 0, int pageSize = 20) 
        {
            var calendarIdentifiers = await _calendarIdentifierService.GetCalendarIdentifiersAsync(page, pageSize);
            return new JsonResult(calendarIdentifiers);
        }
    }
}