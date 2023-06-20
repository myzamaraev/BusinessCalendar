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
            var calendar = await _calendarManagementService.GetCalendar(type, key, year);

            return new JsonResult(calendar);
        }

        [HttpPut]
        [Route("[action]")]
        public async Task SaveCalendar([FromBody]SaveCalendarRequest request)
        {
            var calendar = new Calendar(request.Type, request.Key, request.Year);
            calendar.Dates.Clear();
            calendar.Dates.AddRange(request.Dates);

            await _calendarManagementService.SaveCalendar(calendar);   
        }

        [HttpPut]
        [Route("[action]")]
        public async Task SaveCompactCalendar(SaveCompactCalendarRequest request)
        {
            var compactCalendar = new CompactCalendar(
                new CalendarId 
                { 
                    Type = request.Type, 
                    Key = request.Key, 
                    Year = request.Year 
                },
                request.Holidays,
                request.ExtraWorkDays);

            await _calendarManagementService.SaveCalendar(compactCalendar);   
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<JsonResult> CreateTestCalendar()
        {
            var calendar = new Calendar(CalendarType.State, "US", 2023);

            //doesn't work anymore, because of struct value type. It's forbidden to change value in enumerator
            calendar.Dates.ForEach(x => {
                if (x.Date >= new DateOnly(2023,01,01) && x.Date < new DateOnly(2023,01,9)) 
                    x.IsWorkday = false;
                });

            //doesn't work either
            // foreach (var date in calendar.Dates.Where(x =>
            //     x.Date >= new DateOnly(2023,01,01) && x.Date < new DateOnly(2023,01,9)))
            // {
            //     date.IsWorkday = false;
            // }

            await _calendarManagementService.SaveCalendar(calendar);

            return new JsonResult(calendar);
        }
    }
}