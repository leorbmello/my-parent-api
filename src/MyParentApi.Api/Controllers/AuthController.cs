using Microsoft.AspNetCore.Mvc;
using MyParentApi.Application.DTOs.Requests;
using MyParentApi.Application.Interfaces;

namespace MyParentApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IAuthService authService;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService)
        {
            this.logger = logger;
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

        /*[HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            authService.Logout(new AuthRefreshRequest(token));            
            return Ok("Logout bem-sucedido");
        }*/
    }
}
