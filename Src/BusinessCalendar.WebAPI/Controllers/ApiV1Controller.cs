using Microsoft.AspNetCore.Mvc;

namespace BusinessCalendar.WebAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
public class ApiV1Controller : ControllerBase
{
    
}