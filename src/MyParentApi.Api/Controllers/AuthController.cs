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
        public async Task<IActionResult> LoginAsync([FromBody] AuthRequest request)
        {
            return Ok(await authService.AuthUserAsync(request));
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
        {
            return Ok(await authService.CreateUserAsync(request));
        }

        [HttpPost("recovery-password")]
        public async Task<IActionResult> PasswordRecorverAsync([FromBody] PasswordRecoveryRequest request)
        {
            return Ok(await authService.RecoveryPasswordAsync(request));
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
