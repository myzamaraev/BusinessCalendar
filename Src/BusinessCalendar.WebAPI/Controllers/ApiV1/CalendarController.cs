using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Responses;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCalendar.WebAPI.Controllers.ApiV1
{
    [AllowAnonymous]
    public class CalendarController : ApiV1Controller
    {
        private readonly IClientCalendarService _clientCalendarService;

        public CalendarController(IClientCalendarService clientCalendarService)
        {
            _clientCalendarService = clientCalendarService;
        }

        [HttpGet]
        [Route("{identifier}/{year}")]
        [ProducesResponseType(typeof(GetCalendarResponse), 200)]
        public async Task<ActionResult<GetCalendarResponse>> Get([FromRoute]string identifier, [FromRoute]int year, CancellationToken cancellationToken)
        {
            var response = await _clientCalendarService.GetCalendarAsync(identifier, year, cancellationToken);
            return Ok(response);
        }
        
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(GetCalendarDateResponse), 200)]
        public async Task<ActionResult<GetCalendarDateResponse>> GetDate(string identifier, DateOnly date, CancellationToken cancellationToken)
        {
            var response = await _clientCalendarService.GetCalendarDateAsync(identifier, date, cancellationToken);
            return Ok(response);
        }
    }
}