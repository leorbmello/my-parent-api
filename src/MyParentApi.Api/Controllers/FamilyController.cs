using Microsoft.AspNetCore.Mvc;

namespace MyParentApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyController : ControllerBase
    {
        [HttpGet("query")]
        public async Task<IActionResult> GetAsync()
        {

        }
    }
}
