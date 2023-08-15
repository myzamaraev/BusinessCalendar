using Microsoft.AspNetCore.Mvc;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;

namespace BusinessCalendar.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManagementController : ControllerBase
    {
        private readonly ICalendarManagementService _calendarManagementService;

        public ManagementController(ICalendarManagementService calendarManagementService)
        {
            _calendarManagementService = calendarManagementService;
        }
        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(Calendar), 200)]
        public async Task<JsonResult> GetCalendar(CalendarType type, string key, int year)
        {
            var calendar = await _calendarManagementService.GetCalendarAsync(type, key, year);

            return new JsonResult(calendar);
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
        public async Task<ActionResult> SaveCompactCalendar(SaveCompactCalendarRequest request)
        {
            await _calendarManagementService.SaveCalendarAsync(request);
            return Ok();
        }
    }
}