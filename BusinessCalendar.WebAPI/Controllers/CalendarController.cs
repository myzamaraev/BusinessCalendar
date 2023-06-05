using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCalendar.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        [HttpGet]
        public JsonResult Get(int year, string type, string key)
        {
            var calendar  = new {
                    holidays = new[] {
                        new DateTime(2023,1,1),
                        new DateTime(2023,1,2)
                    },
                    workdays = new DateTime[] {}
                };

            var result = new {
                year,
                type,
                key,
                calendar
            };

            return new JsonResult(result);
        }
    }
}