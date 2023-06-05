using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Dto;
using System.Linq;

namespace BusinessCalendar.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : ControllerBase
    {
        private readonly ICalendarManagementService _calendarManagementService;

        public ManagementController(ICalendarManagementService calendarManagementService)
        {
            _calendarManagementService = calendarManagementService;
        }
        
        [HttpGet]
        [Route("[action]")]
        public JsonResult GetCalendar(CalendarType type, string key, int year)
        {
            var calendar = _calendarManagementService.GetCalendar(type, key, year);

            return new JsonResult(calendar);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<JsonResult> CreateTestCalendar()
        {
            var calendar = new Calendar(CalendarType.State, "US", 2023);
            calendar.Dates.Where(x => x.Date >= new DateTime(2023,01,01)
                && x.Date < new DateTime(2023,01,9))
                .ToList()
                .ForEach(x => x.IsWorkday = false);
            await _calendarManagementService.SaveCalendar(calendar);

            return new JsonResult(calendar);
        }
    }
}