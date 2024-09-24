using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyParentApi.Application.DTOs.Requests.Family;
using MyParentApi.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyParentApi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyController : ControllerBase
    {
        private readonly IFamilyService familyService;

        public FamilyController(IFamilyService familyService) 
        {
            this.familyService = familyService;
        }

        [HttpGet("query/{familyId}")]
        public async Task<IActionResult> GetFamilyAsync(int familyId)
        {
            return Ok(await familyService.GetFamilyByIdAsync(familyId));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateFamilyAsync([FromBody] FamilyCreateRequest request)
        {
            return Ok(await familyService.CreateNewFamilyAsync(request));
        }

        [HttpPost("delete-by-id")]
        public async Task<IActionResult> DeleteByIdAsync([FromBody] FamilyDeleteRequest request)
        {
            return Ok(await familyService.DeleteFamilyAsync(request));
        }

        [HttpPost("delete-by-creator-id/{userId}")]
        public async Task<IActionResult> DeleteByCreatorIdAsync([Required] int userId)
        {
            return Ok(await familyService.DeleteFamilyByCreatorIdAsync(userId));
        }
    }
}
