using Microsoft.AspNetCore.Mvc;
using MyParentApi.Application.DTOs.Requests.Auth;
using MyParentApi.Application.DTOs.Requests.Profile;
using MyParentApi.Application.DTOs.Requests.Users;
using MyParentApi.Application.Interfaces;

namespace MyParentApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IProfileService profileService;

        public AuthController(IAuthService authService,
            IProfileService profileService)
        {
            this.authService = authService;
            this.profileService = profileService;
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


        [HttpPost("change-password-by-rec")]
        public async Task<IActionResult> ChangePasswordByRecAsync([FromBody] PasswordChangeRequest request)
        {
            return Ok(await profileService.ChangePasswordByTokenAsync(request));
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
