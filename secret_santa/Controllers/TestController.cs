using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace secret_santa.Controllers;

[Route("api/")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("protected")]
    [Authorize]
    public IActionResult GetProtectedData()
    {
        return Ok("This is protected data!");
    }

    [HttpGet("public")]
    public IActionResult GetPublicData()
    {
        return Ok("This is public data.");
    }
}