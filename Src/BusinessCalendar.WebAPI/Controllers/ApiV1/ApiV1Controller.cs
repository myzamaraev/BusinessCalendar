using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCalendar.WebAPI.Controllers.ApiV1;

[Route("api/v1/[controller]")]
[Authorize]
public class ApiV1Controller : ApiControllerBase
{
    
}