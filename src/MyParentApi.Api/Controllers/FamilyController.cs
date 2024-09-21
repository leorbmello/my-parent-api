using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyParentApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyController : ControllerBase
    {
        [Authorize]
        [HttpGet("query")]
        public async Task<IActionResult> GetAsync()
        {
            return Ok("Test");
        }
    }
}
