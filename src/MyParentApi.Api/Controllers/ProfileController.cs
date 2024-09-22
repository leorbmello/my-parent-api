using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyParentApi.Application.DTOs.Requests.Profile;
using MyParentApi.Application.Interfaces;

namespace MyParentApi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService profileService) 
        {
            this.profileService = profileService;
        }

        [HttpPost("pswrd-change")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] PasswordChangeRequest request)
        {
            return Ok(await profileService.ChangePasswordAsync(request));
        }
    }
}
