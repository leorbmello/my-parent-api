using Microsoft.AspNetCore.Mvc;
using MyParentApi.Application.DTOs.Requests;
using MyParentApi.Application.Interfaces;

namespace MyParentApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var result = authService.Authenticate(request);
            if (string.IsNullOrEmpty(result.Token)) 
            {
                return Unauthorized(result.ErrorCode);
            }

            return Ok(result);
        }
    }
}
