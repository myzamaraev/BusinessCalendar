using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Dto;
using System.Linq;
using BusinessCalendar.WebAPI.ViewModels;

namespace BusinessCalendar.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalendarIdentifierController : ControllerBase
    {
        private readonly ICalendarManagementService _calendarManagementService;

        public CalendarIdentifierController(ICalendarManagementService calendarManagementService)
        {
            _calendarManagementService = calendarManagementService;
        }

        [HttpPost]
        public async Task CalendarIdentifier([FromBody]AddCalendarRequest request) 
        {
            await _calendarManagementService.AddCalendarIdentifierAsync(request.Type, request.Key);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<CalendarIdentifier>), 200)]
        public async Task<JsonResult> List(int page = 0, int pageSize = 20) 
        {
            var calendarIdentifiers = await _calendarManagementService.GetCalendarIdentifiersAsync(page, pageSize);
            return new JsonResult(calendarIdentifiers);
        }
    }
}