using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCalendar.WebAPI.Controllers.BffV1;

[Route("bff/v1/[controller]")]
[Authorize]
public class BffV1Controller : ApiControllerBase
{
    
}